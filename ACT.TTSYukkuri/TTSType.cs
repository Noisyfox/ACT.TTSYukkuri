namespace ACT.TTSYukkuri
{
    /// <summary>
    /// TTSの種類
    /// </summary>
    public static class TTSType
    {
        /// <summary>
        /// Yukkuri:ゆっくり
        /// </summary>
        public const string Yukkuri = "Yukkuri";

        /// <summary>
        /// Sasara:さとう ささら
        /// </summary>
        public const string SasaraSato = "Sasara";

        /// <summary>
        /// コンボボックスコレクション
        /// </summary>
        public static ComboBoxItem[] ToComboBox = new ComboBoxItem[]
        {
            new ComboBoxItem("ゆっくり", TTSType.Yukkuri),
            new ComboBoxItem("さとう ささら", TTSType.SasaraSato),
        };
    }

    public class ComboBoxItem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="display">表示用</param>
        /// <param name="value">値用</param>
        public ComboBoxItem(
            string display,
            string value)
        {
            this.Display = display;
            this.Value = value;
        }

        /// <summary>
        /// 表示用メンバ
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 値用メンバ
        /// </summary>
        public string Value { get; set; }
    }
}
