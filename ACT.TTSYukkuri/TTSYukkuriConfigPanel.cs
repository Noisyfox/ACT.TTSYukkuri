namespace ACT.TTSYukkuri
{
    using System;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;

    /// <summary>
    /// TTSゆっくり設定Panel
    /// </summary>
    public partial class TTSYukkuriConfigPanel : UserControl
    {
        /// <summary>
        /// TTS設定Panel
        /// </summary>
        private UserControl ttsSettingsPanel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TTSYukkuriConfigPanel()
        {
            this.InitializeComponent();

            this.ttsShuruiComboBox.DisplayMember = "Display";
            this.ttsShuruiComboBox.ValueMember = "Value";
            this.ttsShuruiComboBox.DataSource = TTSType.ToComboBox;

            this.ttsShuruiComboBox.TextChanged += (s1, e1) =>
            {
                TTSYukkuriConfig.Default.TTS = (this.ttsShuruiComboBox.SelectedItem as ComboBoxItem).Value;
                TTSYukkuriConfig.Default.Save();
                this.LoadTTS();
            };
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void TTSYukkuriConfigPanel_Load(object sender, EventArgs e)
        {
            this.ttsShuruiComboBox.SelectedValue = TTSYukkuriConfig.Default.TTS;
            this.LoadTTS();
        }

        /// <summary>
        /// TTSを読み込む
        /// </summary>
        private void LoadTTS()
        {
            try
            {
                // TTSを初期化する
                SpeechController.Default.Initialize();

                // 前のPanelを除去する
                if (this.ttsSettingsPanel != null)
                {
                    this.ttsSettingsGroupBox.Controls.Remove(this.ttsSettingsPanel);
                }

                // 新しいPanelをセットする
                var ttsSettingsPanelNew = SpeechController.Default.TTSSettingsPanel;
                if (ttsSettingsPanelNew != null)
                {
                    ttsSettingsPanelNew.Dock = DockStyle.Fill;
                    this.ttsSettingsGroupBox.Controls.Add(ttsSettingsPanelNew);

                    this.ttsSettingsPanel = ttsSettingsPanelNew;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "TTSの初期化中に例外が発生しました。環境を確認してください" + Environment.NewLine + Environment.NewLine +
                    ex.ToString(),
                    "TTSゆっくりプラグイン",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }
    }
}
