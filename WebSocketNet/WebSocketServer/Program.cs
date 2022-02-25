
using System;
using System.Runtime.InteropServices;

namespace WebSocketServer
{
    class Program
    {
        #region [DllImport] Disable Close Key
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        public static void Main(string[] args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            Console.WriteLine("=============================================================");
            Console.WriteLine($"WebSocket Server Started: Version [1.0]");
            Console.WriteLine("=============================================================");

            try
            {
                WSServer.Instance.Start();

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Type [exit] to quit. Anything else gets broadcast to all users.");
                    Console.Write("> ");

                    var line = Console.ReadLine();

                    if (string.Compare(line, "exit") == 0)
                        break;
                    else
                        WSServer.Instance.BroadCast(line);
                }
                WSServer.Instance.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
