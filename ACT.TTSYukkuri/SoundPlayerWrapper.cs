namespace ACT.TTSYukkuri
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// DirectXでサウンドを再生する
    /// </summary>
    public static class SoundPlayerWrapper
    {
        /// <summary>
        /// サウンド再生する
        /// </summary>
        /// <param name="stream">再生するストリーム</param>
        /// <param name="delay">再生までのディレイ(秒)</param>
        public static void PlayWithDelay(
            Stream stream,
            int delay)
        {
            if (delay == 0)
            {
                Play(stream);
                return;
            }

            var timer = new Timer(new TimerCallback((state) =>
            {
                Play(stream);
                (state as Timer).Dispose();
            }));

            timer.Change(
                delay * 1000,
                0);
        }

        /// <summary>
        /// サウンド再生する
        /// </summary>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="delay">再生までのディレイ(秒)</param>
        public static void PlayWithDelay(
            string file,
            int delay)
        {
            if (delay == 0)
            {
                Play(file);
                return;
            }

            var timer = new Timer(new TimerCallback((state) =>
            {
                Play(file);
                (state as Timer).Dispose();
            }));

            timer.Change(
                delay * 1000,
                0);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="stream">再生するストリーム</param>
        public static void Play(
            Stream stream)
        {
            var file = Path.GetTempFileName();

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }

            PlayCore(file, true);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="stream">再生するストリーム</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            Stream stream,
            int volume)
        {
            var file = Path.GetTempFileName();

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }

            PlayCore(file, true, volume);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="file">再生するサウンドファイル</param>
        public static void Play(
            string file)
        {
            PlayCore(file, false);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="isFileDelete">ファイルを削除するか？</param>
        private static void PlayCore(
            string file,
            bool isFileDelete)
        {
            if (!File.Exists(file))
            {
                return;
            }

            var pi = new ProcessStartInfo()
            {
                FileName = "SoundPlayer.exe",
                Arguments = file + " " + isFileDelete,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var p = new Process()
            {
                StartInfo = pi
            };

            p.Exited += (s, e) =>
            {
                p.Dispose();
            };

            p.Start();
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="isFileDelete">ファイルを削除するか？</param>
        /// <param name="volume">ボリューム</param>
        private static void PlayCore(
            string file,
            bool isFileDelete,
            int volume)
        {
            if (!File.Exists(file))
            {
                return;
            }

            var pi = new ProcessStartInfo()
            {
                FileName = "SoundPlayer.exe",
                Arguments = file + " " + isFileDelete + " " + volume.ToString(),
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var p = new Process()
            {
                StartInfo = pi
            };

            p.Exited += (s, e) =>
            {
                p.Dispose();
            };

            p.Start();
        }
    }
}
