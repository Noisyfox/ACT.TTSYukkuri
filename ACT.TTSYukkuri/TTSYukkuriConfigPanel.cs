namespace ACT.TTSYukkuri
{
    using System;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.SoundPlayer;

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
        /// ロード完了
        /// </summary>
        private bool Loaded;

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
                if (this.Loaded)
                {
                    this.SaveSettings();

                    // 再生デバイスの選択の使用状況を切り替える
                    if (TTSYukkuriConfig.Default.TTS == TTSType.Boyomichan)
                    {
                        this.saiseiDeviceGroupBox.Enabled = false;
                    }
                    else
                    {
                        this.saiseiDeviceGroupBox.Enabled = true;
                    }

                    this.LoadTTS();
                }
            };
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void TTSYukkuriConfigPanel_Load(object sender, EventArgs e)
        {
            // 再生デバイスコンボボックスを設定する
            this.mainDeviceComboBox.DisplayMember = "Name";
            this.mainDeviceComboBox.ValueMember = "Number";
            this.mainDeviceComboBox.DataSource = NAudioPlayer.EnumlateDevices();

            this.subDeviceComboBox.DisplayMember = "Name";
            this.subDeviceComboBox.ValueMember = "Number";
            this.subDeviceComboBox.DataSource = NAudioPlayer.EnumlateDevices();

            this.mainDeviceComboBox.SelectedValue = TTSYukkuriConfig.Default.MainDeviceNo;
            this.enabledSubDeviceCheckBox.Checked = TTSYukkuriConfig.Default.EnabledSubDevice;
            this.subDeviceComboBox.SelectedValue = TTSYukkuriConfig.Default.SubDeviceNo;

            this.subDeviceComboBox.Enabled = this.enabledSubDeviceCheckBox.Checked;

            this.mainDeviceComboBox.TextChanged += (s1, e1) =>
            {
                this.SaveSettings();
            };

            this.enabledSubDeviceCheckBox.CheckedChanged += (s1, e1) =>
            {
                var c = s1 as CheckBox;
                this.subDeviceComboBox.Enabled = c.Checked;
                this.SaveSettings();
            };

            this.subDeviceComboBox.TextChanged += (s1, e1) =>
            {
                this.SaveSettings();
            };

            this.ttsShuruiComboBox.SelectedValue = TTSYukkuriConfig.Default.TTS;
            if (TTSYukkuriConfig.Default.TTS == TTSType.Boyomichan)
            {
                this.saiseiDeviceGroupBox.Enabled = false;
            }
            else
            {
                this.saiseiDeviceGroupBox.Enabled = true;
            }

            this.LoadTTS();

            // オプションのロードを呼出す
            this.LoadOptions();

            this.Loaded = true;
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveSettings()
        {
            TTSYukkuriConfig.Default.TTS = (this.ttsShuruiComboBox.SelectedItem as ComboBoxItem).Value;

            TTSYukkuriConfig.Default.MainDeviceNo = (int)this.mainDeviceComboBox.SelectedValue;
            TTSYukkuriConfig.Default.EnabledSubDevice = this.enabledSubDeviceCheckBox.Checked;
            TTSYukkuriConfig.Default.SubDeviceNo = (int)this.subDeviceComboBox.SelectedValue;

            this.SaveSettingsOptions();

            TTSYukkuriConfig.Default.Save();
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

                // TTSをゆっくりに戻す
                TTSYukkuriConfig.Default.TTS = TTSType.Yukkuri;
                TTSYukkuriConfig.Default.Save();
                this.ttsShuruiComboBox.SelectedValue = TTSYukkuriConfig.Default.TTS;
            }
        }
    }
}
