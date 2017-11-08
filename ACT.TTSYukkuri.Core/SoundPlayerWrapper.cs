using System;
using System.IO;
using System.Threading.Tasks;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using FFXIV.Framework.Common;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// DirectXでサウンドを再生する
    /// </summary>
    public static class SoundPlayerWrapper
    {
        private static string lastPlayWaveFileMain;
        private static DateTime lastPlayTimestampMain;
        private static string lastPlayWaveFileSub;
        private static DateTime lastPlayTimestampSub;

        public static void Play(
            string waveFile)
        {
            var volume = Settings.Default.WaveVolume / 100f;

            if (Settings.Default.EnabledSubDevice)
            {
                if (waveFile == lastPlayWaveFileSub &&
                    (DateTime.Now - lastPlayTimestampSub).TotalSeconds
                    <= Settings.Default.GlobalSoundInterval)
                {
                    return;
                }

                lastPlayTimestampSub = DateTime.Now;
                lastPlayWaveFileSub = waveFile;

                SoundPlayerWrapper.PlayCore(
                    waveFile,
                    volume,
                    Settings.Default.Player,
                    Settings.Default.SubDeviceID);
            }

            if (waveFile == lastPlayWaveFileMain &&
                (DateTime.Now - lastPlayTimestampMain).TotalSeconds
                <= Settings.Default.GlobalSoundInterval)
            {
                return;
            }

            lastPlayTimestampMain = DateTime.Now;
            lastPlayWaveFileMain = waveFile;

            SoundPlayerWrapper.PlayCore(
                waveFile,
                volume,
                Settings.Default.Player,
                Settings.Default.MainDeviceID);
        }

        private static void PlayCore(
            string file,
            float volume = 1.0f,
            WavePlayerTypes playerType = WavePlayerTypes.WASAPI,
            string deviceID = null)
        {
            if (!File.Exists(file))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(deviceID))
            {
                return;
            }

            if (deviceID == PlayDevice.DiscordDeviceID)
            {
                Task.Run(() => DiscordClientModel.Instance.Play(file));
                return;
            }

            WavePlayer.Instance.Play(
                file,
                volume,
                Settings.Default.Player,
                deviceID);
        }
    }
}
