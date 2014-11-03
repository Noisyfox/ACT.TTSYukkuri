namespace ACT.TTSYukkuri.Config
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using ACT.TTSYukkuri.Sasara;

    /// <summary>
    /// TTSYukkuri設定
    /// </summary>
    [Serializable]
    public class TTSYukkuriConfig
    {
        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        [XmlIgnore]
        private static object lockObject = new object();

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
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

        /// <summary>
        /// 設定ファイルのパスを返す
        /// </summary>
        [XmlIgnore]
        public static string FilePath
        {
            get
            {
                var r = string.Empty;

                r = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "anoyetta\\ACT\\ACT.TTSYukkuri.config");

                return r;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TTSYukkuriConfig()
        {
            this.SasaraSettings = new SasaraConfig();

            this.TTS = TTSType.Yukkuri;
        }

        /// <summary>
        /// TTSの種類
        /// </summary>
        public string TTS { get; set; }

        /// <summary>
        /// ゆっくりのスピード
        /// </summary>
        public int YukkuriSpeed { get; set; }

        /// <summary>
        /// ゆっくりのボリューム調整を有効にする
        /// </summary>
        public bool EnabledYukkuriVolumeSetting { get; set; }

        /// <summary>
        /// ゆっくりのボリューム
        /// </summary>
        public int YukkuriVolume { get; set; }

        /// <summary>
        /// ささら設定
        /// </summary>
        public SasaraConfig SasaraSettings { get; set; }

        /// <summary>
        /// 設定をロードする
        /// </summary>
        public void Load()
        {
            var file = FilePath;

            if (File.Exists(file))
            {
                using (var sr = new StreamReader(file, new UTF8Encoding(false)))
                {
                    var xs = new XmlSerializer(typeof(TTSYukkuriConfig));
                    instance = (TTSYukkuriConfig)xs.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 設定をセーブする
        /// </summary>
        public void Save()
        {
            var file = FilePath;

            var dir = Path.GetDirectoryName(file);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var sw = new StreamWriter(file, false, new UTF8Encoding(false)))
            {
                var xs = new XmlSerializer(typeof(TTSYukkuriConfig));
                xs.Serialize(sw, Default);
            }
        }

        /// <summary>
        /// ささらを設定する
        /// </summary>
        public void SetSasara()
        {
            var talker = SasaraSpeechController.Talker;

            talker.Cast = TTSYukkuriConfig.Default.SasaraSettings.Cast;
            talker.Volume = TTSYukkuriConfig.Default.SasaraSettings.Onryo;
            talker.Speed = TTSYukkuriConfig.Default.SasaraSettings.Hayasa;
            talker.Tone = TTSYukkuriConfig.Default.SasaraSettings.Takasa;
            talker.Alpha = TTSYukkuriConfig.Default.SasaraSettings.Seishitsu;

            foreach (var componet in TTSYukkuriConfig.Default.SasaraSettings.Components)
            {
                var componetOnSasara = talker.Components
                    .Where(x => x.Id == componet.Id).FirstOrDefault();

                if (componetOnSasara != null)
                {
                    componetOnSasara.Value = componet.Value;
                }
            }
        }
    }
}
