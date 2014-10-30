namespace ACT.TTSYukkuri
{
    using System;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Properties;

    /// <summary>
    /// TTSゆっくり設定Panel
    /// </summary>
    public partial class TTSYukkuriConfigPanel : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TTSYukkuriConfigPanel()
        {
            this.InitializeComponent();

            this.ttsShuruiComboBox.DisplayMember = "Display";
            this.ttsShuruiComboBox.ValueMember = "Value";
            this.ttsShuruiComboBox.DataSource = TTSType.ToComboBox;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void TTSYukkuriConfigPanel_Load(object sender, EventArgs e)
        {
            this.ttsShuruiComboBox.SelectedValue = Settings.Default.TTSType;
        }

        /// <summary>
        /// TTSの種類 TextChanged
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void ttsShuruiComboBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.TTSType = (this.ttsShuruiComboBox.SelectedItem as ComboBoxItem).Value;
            Settings.Default.Save();
        }
    }
}
