using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using ACT.TTSYukkuri.Common;
using ACT.TTSYukkuri.resources;
using ACT.TTSYukkuri.SoundPlayer;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config
{
    /// <summary>
    /// TTSYukkuri設定
    /// </summary>
    [Serializable]
    public class TTSYukkuriConfig :
        BindableBase
    {
        #region Singleton

        [XmlIgnore]
        private static object lockObject = new object();

        [XmlIgnore]
        private static TTSYukkuriConfig instance;

        /// <summary>
        /// シングルトンインスタンスを返す
        /// </summary>
        [XmlIgnore]
        public static TTSYukkuriConfig Default
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new TTSYukkuriConfig();
                        instance.Load();
                    }

                    return instance;
                }
            }
        }

        #endregion Singleton

        /// <summary>
        /// 設定ファイルのパスを返す
        /// </summary>
        [XmlIgnore]
        public static string FilePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "anoyetta\\ACT\\ACT.TTSYukkuri.config");

        private DateTime lastUpdateDateTime = DateTime.Now;
        private string tts = TTSType.Yukkuri;
        private bool waveCacheClearEnable;
        private int waveVolume = 100;
        private WavePlayers player = WavePlayers.WASAPI;
        private string mainDeviceID;
        private bool enabledSubDevice;
        private string subDeviceID;
        private string boyomiServer = "localhost";
        private int boyomiPort = 50001;
        private YukkuriConfig yukkuriSettings = new YukkuriConfig();
        private HOYAConfig hoyaSettings = new HOYAConfig();
        private OpenJTalkConfig openJTalkSettings = new OpenJTalkConfig();
        private SasaraConfig sasaraSettings = new SasaraConfig();
        private StatusAlertConfig statusAlertSettings = new StatusAlertConfig();
        private DiscordSettings discordSettings = new DiscordSettings();

        private Locales uiLocale = Locales.JA;
        private Locales ffxivLocale = Locales.JA;

        /// <summary>
        /// プラグインのUIのロケール
        /// </summary>
        public Locales UILocale
        {
            get => this.uiLocale;
            set => this.SetProperty(ref this.uiLocale, value);
        }

        /// <summary>
        /// FFXIVのロケール
        /// </summary>
        public Locales FFXIVLocale
        {
            get => this.ffxivLocale;
            set => this.SetProperty(ref this.ffxivLocale, value);
        }

        /// <summary>
        /// 最終アップデート日時
        /// </summary>
        [XmlIgnore]
        public DateTime LastUpdateDateTime
        {
            get => this.lastUpdateDateTime;
            set => this.SetProperty(ref this.lastUpdateDateTime, value);
        }

        /// <summary>
        /// 最終アップデート日時
        /// </summary>
        [XmlElement(ElementName = "LastUpdateDateTime")]
        public string LastUpdateDateTimeCrypted
        {
            get => Crypter.EncryptString(this.lastUpdateDateTime.ToString("o"));
            set
            {
                DateTime d;
                if (DateTime.TryParse(value, out d))
                {
                    if (d > DateTime.Now)
                    {
                        d = DateTime.Now;
                    }

                    this.lastUpdateDateTime = d;
                    return;
                }

                try
                {
                    var decrypt = Crypter.DecryptString(value);
                    if (DateTime.TryParse(decrypt, out d))
                    {
                        if (d > DateTime.Now)
                        {
                            d = DateTime.Now;
                        }

                        this.lastUpdateDateTime = d;
                        return;
                    }
                }
                catch (Exception)
                {
                }

                this.lastUpdateDateTime = DateTime.MinValue;
            }
        }

        /// <summary>
        /// TTSの種類
        /// </summary>
        public string TTS
        {
            get => this.tts;
            set => this.SetProperty(ref this.tts, value);
        }

        /// <summary>
        /// 終了時にキャッシュしたwaveファイルを削除する
        /// </summary>
        public bool WaveCacheClearEnable
        {
            get => this.waveCacheClearEnable;
            set => this.SetProperty(ref this.waveCacheClearEnable, value);
        }

        /// <summary>
        /// Wave再生時のボリューム
        /// </summary>
        public int WaveVolume
        {
            get => this.waveVolume;
            set => this.SetProperty(ref this.waveVolume, value);
        }

        /// <summary>
        /// 再生方式
        /// </summary>
        public WavePlayers Player
        {
            get => this.player;
            set
            {
                if (this.SetProperty(ref this.player, value))
                {
                    this.RaisePropertyChanged(nameof(this.PlayDevices));
                }
            }
        }

        [XmlIgnore]
        public List<PlayDevice> PlayDevices => NAudioPlayer.EnumlateDevices();

        /// <summary>
        /// メイン再生デバイスID
        /// </summary>
        public string MainDeviceID
        {
            get => this.mainDeviceID;
            set => this.SetProperty(ref this.mainDeviceID, value);
        }

        /// <summary>
        /// サブ再生デバイスを有効にする
        /// </summary>
        public bool EnabledSubDevice
        {
            get => this.enabledSubDevice;
            set => this.SetProperty(ref this.enabledSubDevice, value);
        }

        /// <summary>
        /// サブ再生デバイスID
        /// </summary>
        public string SubDeviceID
        {
            get => this.subDeviceID;
            set => this.SetProperty(ref this.subDeviceID, value);
        }

        /// <summary>
        /// 棒読みサーバ
        /// </summary>
        public string BoyomiServer
        {
            get => this.boyomiServer;
            set => this.SetProperty(ref this.boyomiServer, value);
        }

        /// <summary>
        /// 棒読みサーバのポート
        /// </summary>
        public int BoyomiPort
        {
            get => this.boyomiPort;
            set => this.SetProperty(ref this.boyomiPort, value);
        }

        /// <summary>
        /// AquesTalk(ゆっくり)の設定
        /// </summary>
        public YukkuriConfig YukkuriSettings
        {
            get => this.yukkuriSettings;
            set => this.SetProperty(ref this.yukkuriSettings, value);
        }

        /// <summary>
        /// HOYA VoiceTextWebAPI 設定
        /// </summary>
        public HOYAConfig HOYASettings
        {
            get => this.hoyaSettings;
            set => this.SetProperty(ref this.hoyaSettings, value);
        }

        /// <summary>
        /// OpenJTalk設定
        /// </summary>
        public OpenJTalkConfig OpenJTalkSettings
        {
            get => this.openJTalkSettings;
            set => this.SetProperty(ref this.openJTalkSettings, value);
        }

        /// <summary>
        /// ささら設定
        /// </summary>
        public SasaraConfig SasaraSettings
        {
            get => this.sasaraSettings;
            set => this.SetProperty(ref this.sasaraSettings, value);
        }

        /// <summary>
        /// オプション設定
        /// </summary>
        public StatusAlertConfig StatusAlertSettings
        {
            get => this.statusAlertSettings;
            set => this.SetProperty(ref this.statusAlertSettings, value);
        }

        /// <summary>
        /// Discordの設定
        /// </summary>
        public DiscordSettings DiscordSettings
        {
            get => this.discordSettings;
            set => this.SetProperty(ref this.discordSettings, value);
        }

        /// <summary>
        /// 設定をロードする
        /// </summary>
        public void Load()
        {
            lock (lockObject)
            {
                var file = FilePath;

                var activeConfig = this;

                if (File.Exists(file))
                {
                    using (var sr = new StreamReader(file, new UTF8Encoding(false)))
                    {
                        var xs = new XmlSerializer(typeof(TTSYukkuriConfig));
                        instance = (TTSYukkuriConfig)xs.Deserialize(sr);

                        activeConfig = instance;
                    }
                }

                // ステータスアラートの対象を初期化する
                activeConfig?.StatusAlertSettings?.SetDefaultAlertTargets();
            }
        }

        /// <summary>
        /// 設定をセーブする
        /// </summary>
        public void Save()
        {
            lock (lockObject)
            {
                var file = FilePath;

                var dir = Path.GetDirectoryName(file);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                // ステータスアラートの対象を初期化する
                this.StatusAlertSettings.SetDefaultAlertTargets();

                using (var sw = new StreamWriter(file, false, new UTF8Encoding(false)))
                {
                    var xs = new XmlSerializer(typeof(TTSYukkuriConfig));
                    xs.Serialize(sw, Default);
                }
            }
        }
    }
}
