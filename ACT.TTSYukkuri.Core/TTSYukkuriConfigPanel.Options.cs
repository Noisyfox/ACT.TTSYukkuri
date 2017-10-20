namespace ACT.TTSYukkuri
{
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;

    /// <summary>
    /// TTSゆっくり設定Panel オプション設定
    /// </summary>
    public partial class TTSYukkuriConfigPanel
    {
#if false
        /// <summary>
        /// オプションをロードする
        /// </summary>
        private void LoadOptions()
        {
            // 設定をロードする
            this.enableWatchHPCheckBox.Checked = TTSYukkuriConfig.Default.StatusAlertSettings.EnabledHPWatch;
            this.enableWatchMPCheckBox.Checked = TTSYukkuriConfig.Default.StatusAlertSettings.EnabledMPWatch;
            this.enableWatchTPCheckBox.Checked = TTSYukkuriConfig.Default.StatusAlertSettings.EnabledTPWatch;
            this.HPThresholdTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.HPThreshold.ToString();
            this.MPThresholdTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.MPThreshold.ToString();
            this.TPThresholdTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.TPThreshold.ToString();
            this.hpTextToSpeakTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.HPTextToSpeack;
            this.mpTextToSpeakTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.MPTextToSpeack;
            this.tpTextToSpeakTextBox.Text = TTSYukkuriConfig.Default.StatusAlertSettings.TPTextToSpeack;

            var array = new bool[0];

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsHP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                this.watchTargetsHPCheckedListBox.SetItemChecked(i, array[i]);
            }

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsMP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                this.watchTargetsMPCheckedListBox.SetItemChecked(i, array[i]);
            }

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsTP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                this.watchTargetsTPCheckedListBox.SetItemChecked(i, array[i]);
            }

            this.HPThresholdTextBox.Enabled = this.enableWatchHPCheckBox.Checked;
            this.MPThresholdTextBox.Enabled = this.enableWatchMPCheckBox.Checked;
            this.TPThresholdTextBox.Enabled = this.enableWatchTPCheckBox.Checked;
            this.hpTextToSpeakTextBox.Enabled = this.enableWatchHPCheckBox.Checked;
            this.mpTextToSpeakTextBox.Enabled = this.enableWatchMPCheckBox.Checked;
            this.tpTextToSpeakTextBox.Enabled = this.enableWatchTPCheckBox.Checked;
            this.watchTargetsHPCheckedListBox.Enabled = this.enableWatchHPCheckBox.Checked;
            this.watchTargetsMPCheckedListBox.Enabled = this.enableWatchMPCheckBox.Checked;
            this.watchTargetsTPCheckedListBox.Enabled = this.enableWatchTPCheckBox.Checked;

            // イベントを定義する
            this.HPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);
            this.MPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);
            this.TPThresholdTextBox.Leave += (s1, e1) => this.ValidateThresholdTextBox(s1);

            this.enableWatchHPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.HPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.hpTextToSpeakTextBox.Enabled = (s1 as CheckBox).Checked;
                this.watchTargetsHPCheckedListBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.enableWatchMPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.MPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.mpTextToSpeakTextBox.Enabled = (s1 as CheckBox).Checked;
                this.watchTargetsMPCheckedListBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.enableWatchTPCheckBox.CheckedChanged += (s1, e1) =>
            {
                this.TPThresholdTextBox.Enabled = (s1 as CheckBox).Checked;
                this.tpTextToSpeakTextBox.Enabled = (s1 as CheckBox).Checked;
                this.watchTargetsTPCheckedListBox.Enabled = (s1 as CheckBox).Checked;
                this.SaveSettings();
            };

            this.hpTextToSpeakTextBox.Leave += (s1, e1) => this.SaveSettings();
            this.mpTextToSpeakTextBox.Leave += (s1, e1) => this.SaveSettings();
            this.tpTextToSpeakTextBox.Leave += (s1, e1) => this.SaveSettings();

            this.watchTargetsHPCheckedListBox.SelectedIndexChanged += (s1, e1) => this.SaveSettings();
            this.watchTargetsMPCheckedListBox.SelectedIndexChanged += (s1, e1) => this.SaveSettings();
            this.watchTargetsTPCheckedListBox.SelectedIndexChanged += (s1, e1) => this.SaveSettings();
            this.watchTargetsHPCheckedListBox.DoubleClick += (s1, e1) => this.SaveSettings();
            this.watchTargetsMPCheckedListBox.DoubleClick += (s1, e1) => this.SaveSettings();
            this.watchTargetsTPCheckedListBox.DoubleClick += (s1, e1) => this.SaveSettings();
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
            TTSYukkuriConfig.Default.StatusAlertSettings.EnabledHPWatch = this.enableWatchHPCheckBox.Checked;
            TTSYukkuriConfig.Default.StatusAlertSettings.EnabledMPWatch = this.enableWatchMPCheckBox.Checked;
            TTSYukkuriConfig.Default.StatusAlertSettings.EnabledTPWatch = this.enableWatchTPCheckBox.Checked;
            TTSYukkuriConfig.Default.StatusAlertSettings.HPThreshold = int.Parse(this.HPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.StatusAlertSettings.MPThreshold = int.Parse(this.MPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.StatusAlertSettings.TPThreshold = int.Parse(this.TPThresholdTextBox.Text);
            TTSYukkuriConfig.Default.StatusAlertSettings.HPTextToSpeack = this.hpTextToSpeakTextBox.Text;
            TTSYukkuriConfig.Default.StatusAlertSettings.MPTextToSpeack = this.mpTextToSpeakTextBox.Text;
            TTSYukkuriConfig.Default.StatusAlertSettings.TPTextToSpeack = this.tpTextToSpeakTextBox.Text;

            var array = new bool[0];

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsHP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = this.watchTargetsHPCheckedListBox.GetItemChecked(i);
            }

            TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsHP.ItemArray = array;

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsMP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = this.watchTargetsMPCheckedListBox.GetItemChecked(i);
            }

            TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsMP.ItemArray = array;

            array = TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsTP.ItemArray;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = this.watchTargetsTPCheckedListBox.GetItemChecked(i);
            }

            TTSYukkuriConfig.Default.StatusAlertSettings.WatchTargetsTP.ItemArray = array;
        }
#endif
    }
}
