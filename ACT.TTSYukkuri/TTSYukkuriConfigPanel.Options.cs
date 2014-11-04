namespace ACT.TTSYukkuri
{
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;

    /// <summary>
    /// TTSゆっくり設定Panel オプション設定
    /// </summary>
    public partial class TTSYukkuriConfigPanel
    {
        /// <summary>
        /// オプションをロードする
        /// </summary>
        private void LoadOptions()
        {
            // 設定をロードする
            this.enableWatchHPCheckBox.Checked = TTSYukkuriConfig.Default.OptionSettings.EnabledHPWatch;
            this.enableWatchMPCheckBox.Checked = TTSYukkuriConfig.Default.OptionSettings.EnabledMPWatch;
            this.enableWatchTPCheckBox.Checked = TTSYukkuriConfig.Default.OptionSettings.EnabledTPWatch;
            this.HPThresholdTextBox.Text = TTSYukkuriConfig.Default.OptionSettings.HPThreshold.ToString();
            this.MPThresholdTextBox.Text = TTSYukkuriConfig.Default.OptionSettings.MPThreshold.ToString();
            this.TPThresholdTextBox.Text = TTSYukkuriConfig.Default.OptionSettings.TPThreshold.ToString();
            this.enableWatchDebuffCheckBox.Checked = TTSYukkuriConfig.Default.OptionSettings.EnabledDebuffWatch;

            this.HPThresholdTextBox.Enabled = this.enableWatchHPCheckBox.Checked;
            this.MPThresholdTextBox.Enabled = this.enableWatchMPCheckBox.Checked;
            this.TPThresholdTextBox.Enabled = this.enableWatchTPCheckBox.Checked;

            // イベントを定義する
            this.HPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);
            this.MPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);
            this.TPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);

            this.enableWatchHPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.HPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.enableWatchMPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.MPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.enableWatchTPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.TPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.enableWatchDebuffCheckBox.CheckedChanged += (s1, e1) => this.SaveSettings();
        }

        /// <summary>
        /// しきい値テキストボックスを検証する
        /// </summary>
        /// <param name="control">対象のControl</param>
        private void ValidateThresholdTextBox(
            object control)
        {
            var c = control as TextBox;

            int i;
            if (int.TryParse(c.Text, out i))
            {
                if (i <= 0)
                {
                    i = 0;
                }

                if (i >= 100)
                {
                    i = 100;
                }

                c.Text = i.ToString();
            }
            else
            {
                c.Text = "0";
            }

            this.SaveSettings();
        }

        /// <summary>
        /// 設定を保存する(オプション部分)
        /// </summary>
        private void SaveSettingsOptions()
        {
            TTSYukkuriConfig.Default.OptionSettings.EnabledHPWatch = this.enableWatchHPCheckBox.Checked;
            TTSYukkuriConfig.Default.OptionSettings.EnabledMPWatch = this.enableWatchMPCheckBox.Checked;
            TTSYukkuriConfig.Default.OptionSettings.EnabledTPWatch = this.enableWatchTPCheckBox.Checked;
            TTSYukkuriConfig.Default.OptionSettings.HPThreshold = int.Parse(this.HPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.OptionSettings.MPThreshold = int.Parse(this.MPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.OptionSettings.TPThreshold = int.Parse(this.TPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.OptionSettings.EnabledDebuffWatch = this.enableWatchDebuffCheckBox.Checked;
        }
    }
}
