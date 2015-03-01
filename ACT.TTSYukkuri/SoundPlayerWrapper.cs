namespace ACT.TTSYukkuri
{
    using System.IO;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.SoundPlayer;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// DirectXでサウンドを再生する
    /// </summary>
    public static class SoundPlayerWrapper
    {
        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="stream">再生するストリーム</param>
        public static void Play(
            int deviceNo,
            Stream stream)
        {
            Play(
                deviceNo,
                stream,
                100);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="file">再生するファイル</param>
        public static void Play(
            int deviceNo,
            string file)
        {
            Play(
                deviceNo,
                file,
                100);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="stream">再生するストリーム</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            int deviceNo,
            Stream stream,
            int volume)
        {
            var file = Path.GetTempFileName();

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }

            PlayCore(deviceNo, file, true, volume);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="file">再生するファイル</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            int deviceNo,
            string file,
            int volume)
        {
            PlayCore(deviceNo, file, false, volume);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="isFileDelete">ファイルを削除するか？</param>
        /// <param name="volume">ボリューム</param>
        private static void PlayCore(
            int deviceNo,
            string file,
            bool isFileDelete,
            int volume)
        {
            if (!File.Exists(file))
            {
                return;
            }

            ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
            {
                NAudioPlayer.Play(
                    deviceNo,
                    file,
                    isFileDelete,
                    volume);
            });
        }
    }
}
