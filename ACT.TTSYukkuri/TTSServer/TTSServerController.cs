namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Threading;

    using ACT.TTSYukkuri.TTSServer.Core;

    public static class TTSServerController
    {
        private static IpcClientChannel channel;

        public static TTSMessage Message
        {
            get;
            private set;
        }

        private static Process ServerProcess { get; set; }

        private static string ServerProcessPath
        {
            get
            {
                var p = string.Empty;

                var dir = TTSYukkuriPlugin.PluginDirectory;
                p = System.IO.Path.Combine(
                    dir,
                    @"ACT.TTSYukkuri.TTSServer.exe");

                return p;
            }
        }

        public static void Start()
        {
            var pi = new ProcessStartInfo(ServerProcessPath)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            ServerProcess = Process.Start(pi);

            channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, true);

            Message = (TTSMessage)Activator.GetObject(typeof(TTSMessage), "ipc://TTSYukkuriChannel/message");

            Thread.Sleep(500);
        }

        public static void End()
        {
            if (Message != null)
            {
                Message.End();
                Message = null;

                if (channel != null)
                {
                    ChannelServices.UnregisterChannel(channel);
                    channel = null;
                }
            }

            if (ServerProcess != null)
            {
                if (!ServerProcess.HasExited)
                {
                    ServerProcess.Kill();
                }

                ServerProcess.Dispose();
                ServerProcess = null;
            }
        }
    }
}
