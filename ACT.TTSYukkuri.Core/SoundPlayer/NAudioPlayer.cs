using System;
using System.Collections.Generic;
using System.Diagnostics;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using Advanced_Combat_Tracker;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ACT.TTSYukkuri.SoundPlayer
{
    /// <summary>
    /// WavePlayers
    /// </summary>
    public enum WavePlayers
    {
        WaveOut,
        DirectSound,
        WASAPI,
        ASIO,
    }

    /// <summary>
    /// NAudioプレイヤー
    /// </summary>
    public partial class NAudioPlayer
    {
        private const int PlayerLatencyWaveOut = 200;
        private const int PlayerLatencyDirectSoundOut = 200;
        private const int PlayerLatencyWasapiOut = 200;

        /// <summary>
        /// Device Enumrator
        /// </summary>
        private static MMDeviceEnumerator deviceEnumrator = new MMDeviceEnumerator();

        /// <summary>
        /// プレイヤを開放する
        /// </summary>
        public static void DisposePlayers()
        {
        }

        /// <summary>
        /// 再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevices()
        {
            var list = default(List<PlayDevice>);

            switch (TTSYukkuriConfig.Default.Player)
            {
                case WavePlayers.WaveOut:
                    list = EnumlateDevicesByWaveOut();
                    break;

                case WavePlayers.DirectSound:
                    list = EnumlateDevicesByDirectSoundOut();
                    break;

                case WavePlayers.WASAPI:
                    list = EnumlateDevicesByWasapiOut();
                    break;

                case WavePlayers.ASIO:
                    list = EnumlateDevicesByAsioOut();
                    break;
            }

            if (list != null)
            {
                list.Add(PlayDevice.DiscordPlugin);
            }

            return list;
        }

        /// <summary>
        /// WasapiOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByAsioOut()
        {
            var list = new List<PlayDevice>();

            foreach (var name in AsioOut.GetDriverNames())
            {
                list.Add(new PlayDevice()
                {
                    ID = name,
                    Name = name,
                });
            }

            return list;
        }

        /// <summary>
        /// DirectSoundOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByDirectSoundOut()
        {
            var list = new List<PlayDevice>();

            foreach (var device in DirectSoundOut.Devices)
            {
                list.Add(new PlayDevice()
                {
                    ID = device.Guid.ToString(),
                    Name = device.Description,
                });
            }

            return list;
        }

        /// <summary>
        /// WasapiOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByWasapiOut()
        {
            var list = new List<PlayDevice>();

            foreach (var device in deviceEnumrator.EnumerateAudioEndPoints(
                DataFlow.Render,
                DeviceState.Active))
            {
                list.Add(new PlayDevice()
                {
                    ID = device.ID,
                    Name = device.FriendlyName,
                });
            }

            return list;
        }

        /// <summary>
        /// WaveOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByWaveOut()
        {
            var list = new List<PlayDevice>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);
                list.Add(new PlayDevice()
                {
                    ID = i.ToString(),
                    Name = capabilities.ProductName,
                });
            }

            return list;
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceID">再生デバイスID</param>
        /// <param name="waveFile">wavファイル</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            string deviceID,
            string waveFile,
            int volume)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            try
            {
                // Discord指定？
                if (deviceID == PlayDevice.DiscordDeviceID)
                {
                    DiscordClientModel.Instance.Play(waveFile);
                    return;
                }

                var player = GetPlayer(
                    TTSYukkuriConfig.Default.Player,
                    deviceID,
                    waveFile,
                    volume / 100f);

                // 再生する
                player.Play();
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "サウンドの再生で例外が発生しました。");
            }
            finally
            {
#if DEBUG
                sw.Stop();
                Debug.WriteLine(
                    "PlaySound ({0}) -> {1:N0} ticks",
                    TTSYukkuriConfig.Default.Player,
                    sw.ElapsedTicks);
#endif
            }
        }

        private static IWavePlayer GetPlayer(
            WavePlayers playerType,
            string deviceID,
            string wave,
            float volume = 1.0f)
        {
            var player = default(IWavePlayer);

            switch (playerType)
            {
                case WavePlayers.WaveOut:
                    player = new WaveOut()
                    {
                        DeviceNumber = int.Parse(deviceID),
                        DesiredLatency = PlayerLatencyWaveOut,
                    };
                    break;

                case WavePlayers.DirectSound:
                    player = new DirectSoundOut(
                        Guid.Parse(deviceID),
                        PlayerLatencyDirectSoundOut);
                    break;

                case WavePlayers.WASAPI:
                    player = new WasapiOut(
                        deviceEnumrator.GetDevice(deviceID),
                        AudioClientShareMode.Shared,
                        false,
                        PlayerLatencyWasapiOut);
                    break;

                case WavePlayers.ASIO:
                    player = new AsioOut(deviceID);
                    break;
            }

            var audio = new AudioFileReader(wave)
            {
                Volume = volume
            };

            player.Init(audio);
            player.PlaybackStopped += (x, y) =>
            {
                audio.Dispose();
                player.Dispose();
            };

            return player;
        }
    }

    /// <summary>
    /// 再生デバイス
    /// </summary>
    public class PlayDevice
    {
        public const string DiscordDeviceID = "DISCORD";

        public readonly static PlayDevice DiscordPlugin = new PlayDevice()
        {
            ID = DiscordDeviceID,
            Name = "Use Discord",
        };

        /// <summary>
        /// デバイスのID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// デバイス名
        /// </summary>
        public string Name { get; set; }
    }
}
