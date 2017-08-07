#if DEBUG
#if false
    // マルチスタートアップでデバッグするときの定義
#define MULTI_START_DEBUG
#endif
#endif

namespace ACT.TTSYukkuri.TTSServer
{
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
                    EndServer();

                    for (int i = 0; i < 3000; i += 100)
                    {
                        Thread.Sleep(100);
                        if (serverProcess.HasExited)
                        {
                            break;
                        }
                    }

                    if (!serverProcess.HasExited)
                    {
                        serverProcess.Kill();
                    }

                    serverProcess.Dispose();
                    serverProcess = null;
                }
            }
        }

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
                RedirectStandardInput = true,
            };

            serverProcess = Process.Start(pi);
            Thread.Sleep(1000);
#endif
            // TTSサービスへ接続する
            TTSClient.Instance.Open();
        }

        private static void EndServer()
        {
            if (serverProcess != null)
            {
                serverProcess.StandardInput.WriteLine("end");
                serverProcess.StandardInput.Flush();
                serverProcess.StandardInput.Close();

                Thread.Sleep(10);
            }
        }
    }
}
