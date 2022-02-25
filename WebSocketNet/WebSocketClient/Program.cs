
using System;
using System.Runtime.InteropServices;

namespace WebSocketClient
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
            string name = "TIM";

            if (args.Length == 1)
                name = args[0];

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            Console.WriteLine("=============================================================");
            Console.WriteLine($"WebSocket Client Started: Version [1.0]");
            Console.WriteLine("=============================================================");

            try
            {
                WSClient.Instance.Start(name);

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Type [exit] to quit. Anything else gets sent to server.");
                    Console.Write("> ");

                    var line = Console.ReadLine();

                    if (string.Compare(line, "exit") == 0)
                        break;
                    else
                        WSClient.Instance.Send(line);
                }
                WSClient.Instance.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
