namespace ACT.TTSYukkuri.Config
{
    using System;

    using ACT.TTSYukkuri.Sasara;

    /// <summary>
    /// TTSささら設定
    /// </summary>
    [Serializable]
    public class SasaraConfig
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SasaraConfig()
        {
            this.Components = new SasaraSettingsDataSet.SasaraComponentsDataTable();

            this.Cast = "さとうささら";
            this.Onryo = 100;
            this.Hayasa = 50;
            this.Takasa = 50;
            this.Seishitsu = 50;
        }

        /// <summary>
        /// キャスト
        /// </summary>
        public string Cast { get; set; }

        /// <summary>
        /// 感情コンポーネント
        /// </summary>
        public SasaraSettingsDataSet.SasaraComponentsDataTable Components { get; set; }

        /// <summary>
        /// 音量
        /// </summary>
        public uint Onryo { get; set; }

        /// <summary>
        /// 早さ
        /// </summary>
        public uint Hayasa { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        public uint Takasa { get; set; }

        /// <summary>
        /// 声質
        /// </summary>
        public uint Seishitsu { get; set; }
    }
}
