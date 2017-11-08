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
        private static string lastPlayWaveFile;
        private static DateTime lastPlayTimestamp;

        public static void Play(
            string waveFile)
        {
            if (waveFile == lastPlayWaveFile &&
                (DateTime.Now - lastPlayTimestamp).TotalSeconds
                <= Settings.Default.GlobalSoundInterval)
            {
                return;
            }

            lastPlayTimestamp = DateTime.Now;
            lastPlayWaveFile = waveFile;

            var volume = Settings.Default.WaveVolume / 100f;

            if (Settings.Default.EnabledSubDevice)
            {
                SoundPlayerWrapper.PlayCore(
                    waveFile,
                    volume,
                    Settings.Default.Player,
                    Settings.Default.SubDeviceID);
            }

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
