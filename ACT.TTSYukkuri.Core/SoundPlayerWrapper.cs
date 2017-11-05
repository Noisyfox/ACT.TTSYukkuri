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
        public static void Play(
            string waveFile)
        {
            var volume = TTSYukkuriConfig.Default.WaveVolume / 100f;

            if (TTSYukkuriConfig.Default.EnabledSubDevice)
            {
                SoundPlayerWrapper.PlayCore(
                    waveFile,
                    volume,
                    TTSYukkuriConfig.Default.Player,
                    TTSYukkuriConfig.Default.SubDeviceID);
            }

            SoundPlayerWrapper.PlayCore(
                waveFile,
                volume,
                TTSYukkuriConfig.Default.Player,
                TTSYukkuriConfig.Default.MainDeviceID);
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
                TTSYukkuriConfig.Default.Player,
                deviceID);
        }
    }
}
