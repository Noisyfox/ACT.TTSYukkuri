using System.IO;
using System.Windows.Forms;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.SoundPlayer;
using Advanced_Combat_Tracker;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// DirectXでサウンドを再生する
    /// </summary>
    public static class SoundPlayerWrapper
    {
        /// <summary>
        /// メインデバイス・サブデバイスでWAVEを再生する
        /// </summary>
        /// <param name="waveFile"></param>
        public static void Play(
            string waveFile)
        {
            if (TTSYukkuriConfig.Default.EnabledSubDevice)
            {
                SoundPlayerWrapper.Play(
                    TTSYukkuriConfig.Default.SubDeviceID,
                    waveFile);
            }

            SoundPlayerWrapper.Play(
                TTSYukkuriConfig.Default.MainDeviceID,
                waveFile);
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceID">再生デバイスID</param>
        /// <param name="waveFile">wavファイル</param>
        public static void Play(
            string deviceID,
            string waveFile) =>
            SoundPlayerWrapper.PlayCore(
                deviceID,
                waveFile,
                false,
                TTSYukkuriConfig.Default.WaveVolume);

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="file">再生するファイル</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            string deviceID,
            string file,
            int volume) =>
            SoundPlayerWrapper.PlayCore(deviceID, file, false, volume);

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="deviceID">デバイスID</param>
        /// <param name="file">再生するサウンドファイル</param>
        /// <param name="isFileDelete">ファイルを削除するか？</param>
        /// <param name="volume">ボリューム</param>
        private static void PlayCore(
            string deviceID,
            string file,
            bool isFileDelete,
            int volume)
        {
            if (!File.Exists(file))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(deviceID))
            {
                return;
            }

            if (ActGlobals.oFormActMain.InvokeRequired)
            {
                ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                {
                    NAudioPlayer.Play(
                        deviceID,
                        file,
                        isFileDelete,
                        volume);
                });
            }
            else
            {
                NAudioPlayer.Play(
                    deviceID,
                    file,
                    isFileDelete,
                    volume);
            }
        }
    }
}
