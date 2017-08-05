#if DEBUG
// #if !DEBUG
// マルチスタートアップでデバッグするときの定義
#define MULTI_START_DEBUG
#endif

namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using ACT.TTSYukkuri.TTSServer.Core;

    public static class TTSServerController
    {
        private static Process serverProcess;

        private static string ServerProcessPath =>
            Path.Combine(
                TTSYukkuriPlugin.PluginDirectory,
                @"ACT.TTSYukkuri.TTSServer.exe");

        public static void Start()
        {
#if !MULTI_START_DEBUG
            // ゾンビプロセスがいたら殺す
            var ps = Process.GetProcessesByName("ACT.TTSYukkuri.TTSServer");
            if (ps != null)
            {
                foreach (var p in ps)
                {
                    p.Kill();
                    p.Dispose();
                }
            }

            var pi = new ProcessStartInfo(ServerProcessPath)
            {
#if !DEBUG
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
#endif
                UseShellExecute = false,
            };

            serverProcess = Process.Start(pi);
            Thread.Sleep(1000);
#endif
            // TTSサービスへ接続する
            TTSClient.Instance.Open();
        }

        public static void End()
        {
            try
            {
                if (TTSClient.Instance.Channel != null)
                {
                    TTSClient.Instance.Channel.End();
                    Thread.Sleep(500);
                }
            }
            finally
            {
                if (serverProcess != null)
                {
                    if (!serverProcess.HasExited)
                    {
                        serverProcess.Kill();
                    }

                    serverProcess.Dispose();
                    serverProcess = null;
                }
            }
        }
    }
}
