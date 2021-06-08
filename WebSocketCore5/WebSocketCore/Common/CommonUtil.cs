
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WebSocketCore.Common
{
    public static class CommonUtil
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }

        public static uint ToEpochTime(this DateTime date)
        {
            return Convert.ToUInt32((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static DateTime FromEpochTime(this long epochTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(epochTime);
        }

        public static uint FormatDateTime(this DateTime date, string format)
        {
            return Convert.ToUInt32(date.ToString(format));
        }

        // use this to generate sig
        public static string GenerateSignature(string timestamp, string method, string url, string body, string appSecret)
        {
            return GetHMACInHex(appSecret, timestamp + method + url + body);
        }

        public static string GetHMACInHex(string key, string data)
        {
            var hmacKey = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA256(hmacKey))
            {
                var sig = hmac.ComputeHash(dataBytes);
                return ByteToHexString(sig);
            }
        }

        public static string ByteToHexString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            }
            return new string(c);
        }

        /// <summary>
        /// Creates the Access Sign for the API based on the request.
        /// </summary>
        /// <param name="timestamp">The current timestamp since 01-01-1970 epoch.</param>
        /// <param name="command">The HTTP verb of the request.</param>
        /// <param name="path">The path of the API request.</param>
        /// <param name="body">The (optional) body content.</param>
        /// <param name="apiSecret">The API secret.</param>
        /// <returns>The access sign.</returns>
        public static string GetAccessSign(string timestamp, string command, string path, string body, string apiSecret)
        {
            var hmacKey = Encoding.UTF8.GetBytes(apiSecret);

            string data = timestamp + command + path + body;
            using (var signatureStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                return new HMACSHA256(hmacKey).ComputeHash(signatureStream).Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b), sb => sb.ToString());
            }
        }

        public static string ToJsonString(this object _obj, Formatting fmt = Formatting.None)
        {
            string json = JsonConvert.SerializeObject(_obj, fmt, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }

        public static T FromJsonToObject<T>(this string json)
        {
            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static TcpState GetState(this TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)
                                 && x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint)
              );

            return foo != null ? foo.State : TcpState.Unknown;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

        public static int Remove<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }
            return itemsToRemove.Count;
        }

        // _someObservableCollection.Sort(x => x.Number, false); // Where number is an simple property (non-onject)
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector, bool isAZ)
        {
            if (isAZ)
            {
                List<TSource> sortedList = source.OrderBy(keySelector).ToList();
                source.Clear();
                foreach (var sortedItem in sortedList)
                {
                    source.Add(sortedItem);
                }
            }
            else
            {
                List<TSource> sortedList = source.OrderByDescending(keySelector).ToList();
                source.Clear();
                foreach (var sortedItem in sortedList)
                {
                    source.Add(sortedItem);
                }
            }
        }

        public static bool IsInt(object Expression)
        {
            return int.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
        }

        public static bool IsDouble(object Expression)
        {
            return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
        }

        public static void StartProcess(string exepath, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exepath);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = args;
            Process.Start(startInfo);
        }

        public static async Task<long> PerformLatencyTest(string host, int timeout)
        {
            var task = Task.Run(() => {
                try
                {
                    Ping myPing = new Ping();
                    PingReply reply = myPing.Send(host, timeout);

                    if (reply != null && reply.Status == IPStatus.Success)
                        return reply.RoundtripTime;
                }
                catch { }
                return 0;
            });
            return await task;
        }
    }
}
