namespace ACT.TTSYukkuri.TTSServer
{
    using System;

    using ACT.TTSYukkuri.TTSServer.Core;

    public static class Program
    {
        private static TTSServer server;

        public static void Main(string[] args)
        {
            // WCFサービスホストを開始する
            server = new TTSServer();
            server.OpenServer();

            Console.WriteLine(
                $"TTSServer opened. uri:{TTSServerConstants.ServiceEndPointUri}");

            // 終了を待つ
            Console.ReadLine();

            // WCFサービスホストを終了する
            server.End();

            Console.WriteLine(
                "TTSServer closed.");
        }
    }
}
