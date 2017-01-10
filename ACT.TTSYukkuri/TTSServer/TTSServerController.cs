#if DEBUG
// マルチスタートアップでデバッグするときの定義
#define MULTI_START_DEBUG
#endif

namespace ACT.TTSYukkuri.TTSServer
{
    using System.Diagnostics;
    using System.IO;

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
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            serverProcess = Process.Start(pi);
#endif
            // TTSサービスへ接続する
            TTSClient.Instance.Open();
        }

        public static void End()
        {
            if (TTSClient.Instance.Channel != null)
            {
                TTSClient.Instance.Channel.End();
            }

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
