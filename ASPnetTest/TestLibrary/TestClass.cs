using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TestLibrary
{
    public static class TestClass
    {
        public static bool Run(string message)
        {
            if (message.Length == 0)
                return false;

            return true;
        }        

        public static bool WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

        public static string ReadAppSetting(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static string ReadConnectString(string key)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
