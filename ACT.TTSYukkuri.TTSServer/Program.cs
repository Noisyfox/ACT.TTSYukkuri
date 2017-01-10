namespace ACT.TTSYukkuri.TTSServer
{
    using System;

    using ACT.TTSYukkuri.TTSServer.Core;

    public static class Program
    {
        public static void Main(string[] args)
        {
            // WCFサービスホストを開始する
            TTSServer.Instance.OpenServer();

            Console.WriteLine(
                $"TTSServer opened. uri:{TTSServerConstants.ServiceEndPointUri}");

            // 終了を待つ
            Console.ReadLine();

            // WCFサービスホストを終了する
            TTSServer.Instance.End();

            Console.WriteLine(
                "TTSServer closed.");
        }
    }
}
