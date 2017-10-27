using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            foreach (var kvp in playerDictionary)
            {
                foreach (var player in kvp.Value)
                {
                    player.Dispose();
                }
            }

            playerDictionary.Clear();
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
                    waveFile);

                // 再生する
                player?.Play(volume / 100f);
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

        private static IDictionary<string, YukkuriPlayer[]> playerDictionary = new ConcurrentDictionary<string, YukkuriPlayer[]>();

        private static YukkuriPlayer GetPlayer(
            WavePlayers playerType,
            string deviceID,
            string wave)
        {
            var player = default(YukkuriPlayer);

            var key = $"{playerType}-{deviceID}-{wave}";
            if (playerDictionary.ContainsKey(key))
            {
                player = playerDictionary[key].FirstOrDefault(x => !x.IsPlaying);
                return player;
            }

            playerDictionary[key] = new YukkuriPlayer[]
            {
                player = CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
                CreatePlayer(playerType, deviceID, wave),
            };

            return player;
        }

        public static YukkuriPlayer CreatePlayer(
            WavePlayers playerType,
            string deviceID,
            string wave)
        {
            // プレイヤーを使い捨てるか？
            const bool isInstant = false;

            var player = default(YukkuriPlayer);
            switch (playerType)
            {
                case WavePlayers.WaveOut:
                    player = new YukkuriPlayer(
                        new WaveOut()
                        {
                            DeviceNumber = int.Parse(deviceID),
                            DesiredLatency = PlayerLatencyWaveOut,
                        },
                        wave,
                        isInstant);
                    break;

                case WavePlayers.DirectSound:
                    player = new YukkuriPlayer(
                        new DirectSoundOut(
                            Guid.Parse(deviceID),
                            PlayerLatencyDirectSoundOut),
                        wave,
                        isInstant);
                    break;

                case WavePlayers.WASAPI:
                    player = new YukkuriPlayer(
                        new WasapiOut(
                            deviceEnumrator.GetDevice(deviceID),
                            AudioClientShareMode.Shared,
                            false,
                            PlayerLatencyWasapiOut),
                        wave,
                        isInstant);
                    break;

                case WavePlayers.ASIO:
                    player = new YukkuriPlayer(
                        new AsioOut(deviceID),
                        wave,
                        isInstant);
                    break;
            }

            return player;
        }
    }

    public class YukkuriPlayer : IDisposable
    {
        public YukkuriPlayer(
            IWavePlayer player,
            string wave,
            bool instant = false)
        {
            this.InnerPlayer = player;
            this.AudioStream = new AudioFileReader(wave);
            this.Instant = instant;
            this.Init();
        }

        public void Init()
        {
            if (this.InnerPlayer == null ||
                this.AudioStream == null)
            {
                return;
            }

            this.InnerPlayer.PlaybackStopped += (x, y) =>
            {
                this.AudioStream.Position = 0;

                if (this.Instant)
                {
                    this.InnerPlayer?.Dispose();
                    this.AudioStream?.Dispose();
                    this.InnerPlayer = null;
                    this.AudioStream = null;
                }
            };

            this.InnerPlayer.Init(this.AudioStream);
        }

        public void Play(
            float volume = 1.0f)
        {
            if (this.InnerPlayer == null ||
                this.AudioStream == null)
            {
                return;
            }

            if (this.InnerPlayer.PlaybackState == PlaybackState.Playing)
            {
                return;
            }

            this.AudioStream.Volume = volume;
            this.InnerPlayer.Play();
        }

        public void Dispose()
        {
            this.InnerPlayer?.Dispose();
            this.AudioStream?.Dispose();
            this.InnerPlayer = null;
            this.AudioStream = null;
        }

        public bool Instant { get; set; }
        public IWavePlayer InnerPlayer { get; private set; }
        public AudioFileReader AudioStream { get; private set; }
        public bool IsPlaying => this.InnerPlayer?.PlaybackState == PlaybackState.Playing;

        public float Volume
        {
            get => this.AudioStream.Volume;
            set => this.AudioStream.Volume = value;
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
