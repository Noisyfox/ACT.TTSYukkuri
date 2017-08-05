namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.IO;
    using System.Text;

    using ACT.TTSYukkuri.TTSServer.Core;

    public static class Program
    {
        private static string LogFile => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"anoyetta\ACT\ACT.TTSYukkuri.TTSServer.log");

        public static void Main(string[] args)
        {
            // WCFサービスホストを開始する
            TTSServer.Instance.OpenServer();

            Program.WriteLineLog(
                $"TTSServer opened. uri:{TTSServerConstants.ServiceEndPointUri}");

            // 終了を待つ
            Console.ReadLine();

            // WCFサービスホストを終了する
            TTSServer.Instance.End();
            TTSServer.Instance.CloseServer();

            Program.WriteLineLog(
                "TTSServer closed.");
        }

        public static void WriteLineLog(
            string text)
        {
            var log =
                $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}] {text}";

            Console.WriteLine(log);
            Program.WriteCore(log + Environment.NewLine);
        }

        public static void WriteLineLog(
            string text,
            Exception ex)
        {
            var log =
                $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}] {text}" + Environment.NewLine +
                ex.ToString();

            Console.WriteLine(log);
            Program.WriteCore(log + Environment.NewLine);
        }

        private static void WriteCore(
            string text)
        {
            try
            {
                var dir = Path.GetDirectoryName(LogFile);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.AppendAllText(
                    LogFile,
                    text,
                    new UTF8Encoding(false));
            }
            catch (Exception)
            {
            }
        }
    }
}
