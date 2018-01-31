using System;
using System.IO;
using System.Threading.Tasks;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using FFXIV.Framework.Common;

namespace ACT.TTSYukkuri
{
    public enum PlayDevices
    {
        Both = 0,
        Main = 1,
        Sub = 2
    }

    /// <summary>
    /// DirectXでサウンドを再生する
    /// </summary>
    public static class SoundPlayerWrapper
    {
        public static void Play(
            string waveFile,
            PlayDevices playDevice = PlayDevices.Both)
        {
            var volume = Settings.Default.WaveVolume / 100f;

            switch (playDevice)
            {
                case PlayDevices.Both:
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
                    break;

                case PlayDevices.Main:
                    SoundPlayerWrapper.PlayCore(
                        waveFile,
                        volume,
                        Settings.Default.Player,
                        Settings.Default.MainDeviceID);
                    break;

                case PlayDevices.Sub:
                    if (Settings.Default.EnabledSubDevice)
                    {
                        SoundPlayerWrapper.PlayCore(
                            waveFile,
                            volume,
                            Settings.Default.Player,
                            Settings.Default.SubDeviceID);
                    }
                    break;
            }
        }

        private static string lastPlayParameter;
        private static DateTime lastPlayTimestamp;

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

            var playParameter = $"{deviceID}-{file}";
            if (lastPlayParameter == playParameter &&
                (DateTime.Now - lastPlayTimestamp).TotalSeconds
                <= Settings.Default.GlobalSoundInterval)
            {
                return;
            }

            lastPlayParameter = playParameter;
            lastPlayTimestamp = DateTime.Now;

            WavePlayer.Instance.Play(
                file,
                volume,
                Settings.Default.Player,
                deviceID);
        }
    }
}
