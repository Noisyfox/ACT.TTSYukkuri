namespace ACT.TTSYukkuri.Config
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

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
    }
}
