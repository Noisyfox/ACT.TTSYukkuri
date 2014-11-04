namespace ACT.TTSYukkuri.Config
{
    using System;

    /// <summary>
    /// オプション設定
    /// </summary>
    [Serializable]
    public class OptionsConfig
    {
        /// <summary>
        /// HPの監視を有効にする
        /// </summary>
        public bool EnabledHPWatch { get; set; }

        /// <summary>
        /// HP読上げのしきい値
        /// </summary>
        public int HPThreshold { get; set; }

        /// <summary>
        /// MPの監視を有効にする
        /// </summary>
        public bool EnabledMPWatch { get; set; }

        /// <summary>
        /// MP読上げのしきい値
        /// </summary>
        public int MPThreshold { get; set; }

        /// <summary>
        /// TPの監視を有効にする
        /// </summary>
        public bool EnabledTPWatch { get; set; }

        /// <summary>
        /// TP読上げのしきい値
        /// </summary>
        public int TPThreshold { get; set; }

        /// <summary>
        /// デバフの監視を有効にする
        /// </summary>
        public bool EnabledDebuffWatch { get; set; }
    }
}
