namespace ACT.TTSYukkuri
{
    using System;
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
        /// <param name="deviceID">デバイスID</param>
        /// <param name="stream">再生するストリーム</param>
        public static void Play(
            Guid deviceID,
            Stream stream)
        {
            Play(
                deviceID,
                stream,
                100);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="file">再生するファイル</param>
        public static void Play(
            Guid deviceID,
            string file)
        {
            Play(
                deviceID,
                file,
                100);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="stream">再生するストリーム</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            Guid deviceID,
            Stream stream,
            int volume)
        {
            var file = Path.GetTempFileName();

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }

            PlayCore(deviceID, file, true, volume);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="file">再生するファイル</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            Guid deviceID,
            string file,
            int volume)
        {
            PlayCore(deviceID, file, false, volume);
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="isFileDelete">ファイルを削除するか？</param>
        /// <param name="volume">ボリューム</param>
        private static void PlayCore(
            Guid deviceID,
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
                    deviceID,
                    file,
                    isFileDelete,
                    volume);
            });
        }
    }
}
