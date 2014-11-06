namespace ACT.TTSYukkuri
{
    partial class TTSYukkuriConfigPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TTSYukkuriConfigPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.ttsShuruiComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ttsSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.enabledSelfCheckBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tpTextToSpeakTextBox = new System.Windows.Forms.TextBox();
            this.mpTextToSpeakTextBox = new System.Windows.Forms.TextBox();
            this.hpTextToSpeakTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.enableWatchTPCheckBox = new System.Windows.Forms.CheckBox();
            this.enableWatchMPCheckBox = new System.Windows.Forms.CheckBox();
            this.enableWatchHPCheckBox = new System.Windows.Forms.CheckBox();
            this.saiseiDeviceGroupBox = new System.Windows.Forms.GroupBox();
            this.enabledSubDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.subDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mainDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.optionGroupBox.SuspendLayout();
            this.saiseiDeviceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "TTSの種類";
            // 
            // ttsShuruiComboBox
            // 
            this.ttsShuruiComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ttsShuruiComboBox.FormattingEnabled = true;
            this.ttsShuruiComboBox.Location = new System.Drawing.Point(69, 16);
            this.ttsShuruiComboBox.Name = "ttsShuruiComboBox";
            this.ttsShuruiComboBox.Size = new System.Drawing.Size(224, 20);
            this.ttsShuruiComboBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(427, 33);
            this.label3.TabIndex = 3;
            this.label3.Text = "※注意\r\n「CeVIO Creative Studio」は製品版が必要です";
            // 
            // ttsSettingsGroupBox
            // 
            this.ttsSettingsGroupBox.Location = new System.Drawing.Point(5, 126);
            this.ttsSettingsGroupBox.Name = "ttsSettingsGroupBox";
            this.ttsSettingsGroupBox.Size = new System.Drawing.Size(463, 505);
            this.ttsSettingsGroupBox.TabIndex = 1;
            this.ttsSettingsGroupBox.TabStop = false;
            this.ttsSettingsGroupBox.Text = "TTSの設定";
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Controls.Add(this.enabledSelfCheckBox);
            this.optionGroupBox.Controls.Add(this.textBox1);
            this.optionGroupBox.Controls.Add(this.tpTextToSpeakTextBox);
            this.optionGroupBox.Controls.Add(this.mpTextToSpeakTextBox);
            this.optionGroupBox.Controls.Add(this.hpTextToSpeakTextBox);
            this.optionGroupBox.Controls.Add(this.label5);
            this.optionGroupBox.Controls.Add(this.TPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.label4);
            this.optionGroupBox.Controls.Add(this.MPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.label2);
            this.optionGroupBox.Controls.Add(this.HPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.enableWatchTPCheckBox);
            this.optionGroupBox.Controls.Add(this.enableWatchMPCheckBox);
            this.optionGroupBox.Controls.Add(this.enableWatchHPCheckBox);
            this.optionGroupBox.Location = new System.Drawing.Point(474, 126);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.Size = new System.Drawing.Size(720, 505);
            this.optionGroupBox.TabIndex = 3;
            this.optionGroupBox.TabStop = false;
            this.optionGroupBox.Text = "オプション設定";
            // 
            // enabledSelfCheckBox
            // 
            this.enabledSelfCheckBox.AutoSize = true;
            this.enabledSelfCheckBox.Location = new System.Drawing.Point(6, 293);
            this.enabledSelfCheckBox.Name = "enabledSelfCheckBox";
            this.enabledSelfCheckBox.Size = new System.Drawing.Size(180, 16);
            this.enabledSelfCheckBox.TabIndex = 11;
            this.enabledSelfCheckBox.Text = "プレイヤー自身も検出対象にする";
            this.enabledSelfCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(417, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(297, 242);
            this.textBox1.TabIndex = 10;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // tpTextToSpeakTextBox
            // 
            this.tpTextToSpeakTextBox.Location = new System.Drawing.Point(6, 247);
            this.tpTextToSpeakTextBox.Name = "tpTextToSpeakTextBox";
            this.tpTextToSpeakTextBox.Size = new System.Drawing.Size(405, 19);
            this.tpTextToSpeakTextBox.TabIndex = 8;
            // 
            // mpTextToSpeakTextBox
            // 
            this.mpTextToSpeakTextBox.Location = new System.Drawing.Point(6, 156);
            this.mpTextToSpeakTextBox.Name = "mpTextToSpeakTextBox";
            this.mpTextToSpeakTextBox.Size = new System.Drawing.Size(405, 19);
            this.mpTextToSpeakTextBox.TabIndex = 5;
            // 
            // hpTextToSpeakTextBox
            // 
            this.hpTextToSpeakTextBox.Location = new System.Drawing.Point(6, 71);
            this.hpTextToSpeakTextBox.Name = "hpTextToSpeakTextBox";
            this.hpTextToSpeakTextBox.Size = new System.Drawing.Size(405, 19);
            this.hpTextToSpeakTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "%以下になったら読上げる";
            // 
            // TPThresholdTextBox
            // 
            this.TPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TPThresholdTextBox.Location = new System.Drawing.Point(6, 222);
            this.TPThresholdTextBox.MaxLength = 3;
            this.TPThresholdTextBox.Name = "TPThresholdTextBox";
            this.TPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.TPThresholdTextBox.TabIndex = 7;
            this.TPThresholdTextBox.Text = "0";
            this.TPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "%以下になったら読上げる";
            // 
            // MPThresholdTextBox
            // 
            this.MPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MPThresholdTextBox.Location = new System.Drawing.Point(6, 131);
            this.MPThresholdTextBox.MaxLength = 3;
            this.MPThresholdTextBox.Name = "MPThresholdTextBox";
            this.MPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.MPThresholdTextBox.TabIndex = 4;
            this.MPThresholdTextBox.Text = "0";
            this.MPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "%以下になったら読上げる";
            // 
            // HPThresholdTextBox
            // 
            this.HPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.HPThresholdTextBox.Location = new System.Drawing.Point(6, 46);
            this.HPThresholdTextBox.MaxLength = 3;
            this.HPThresholdTextBox.Name = "HPThresholdTextBox";
            this.HPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.HPThresholdTextBox.TabIndex = 1;
            this.HPThresholdTextBox.Text = "0";
            this.HPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // enableWatchTPCheckBox
            // 
            this.enableWatchTPCheckBox.AutoSize = true;
            this.enableWatchTPCheckBox.Location = new System.Drawing.Point(6, 200);
            this.enableWatchTPCheckBox.Name = "enableWatchTPCheckBox";
            this.enableWatchTPCheckBox.Size = new System.Drawing.Size(151, 16);
            this.enableWatchTPCheckBox.TabIndex = 6;
            this.enableWatchTPCheckBox.Text = "PartyのTP低下を監視する";
            this.enableWatchTPCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableWatchMPCheckBox
            // 
            this.enableWatchMPCheckBox.AutoSize = true;
            this.enableWatchMPCheckBox.Location = new System.Drawing.Point(6, 109);
            this.enableWatchMPCheckBox.Name = "enableWatchMPCheckBox";
            this.enableWatchMPCheckBox.Size = new System.Drawing.Size(153, 16);
            this.enableWatchMPCheckBox.TabIndex = 3;
            this.enableWatchMPCheckBox.Text = "PartyのMP低下を監視する";
            this.enableWatchMPCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableWatchHPCheckBox
            // 
            this.enableWatchHPCheckBox.AutoSize = true;
            this.enableWatchHPCheckBox.Location = new System.Drawing.Point(6, 24);
            this.enableWatchHPCheckBox.Name = "enableWatchHPCheckBox";
            this.enableWatchHPCheckBox.Size = new System.Drawing.Size(152, 16);
            this.enableWatchHPCheckBox.TabIndex = 0;
            this.enableWatchHPCheckBox.Text = "PartyのHP低下を監視する";
            this.enableWatchHPCheckBox.UseVisualStyleBackColor = true;
            // 
            // saiseiDeviceGroupBox
            // 
            this.saiseiDeviceGroupBox.Controls.Add(this.enabledSubDeviceCheckBox);
            this.saiseiDeviceGroupBox.Controls.Add(this.subDeviceComboBox);
            this.saiseiDeviceGroupBox.Controls.Add(this.label6);
            this.saiseiDeviceGroupBox.Controls.Add(this.mainDeviceComboBox);
            this.saiseiDeviceGroupBox.Controls.Add(this.label7);
            this.saiseiDeviceGroupBox.Location = new System.Drawing.Point(474, 3);
            this.saiseiDeviceGroupBox.Name = "saiseiDeviceGroupBox";
            this.saiseiDeviceGroupBox.Size = new System.Drawing.Size(720, 117);
            this.saiseiDeviceGroupBox.TabIndex = 2;
            this.saiseiDeviceGroupBox.TabStop = false;
            this.saiseiDeviceGroupBox.Text = "再生デバイス";
            // 
            // enabledSubDeviceCheckBox
            // 
            this.enabledSubDeviceCheckBox.AutoSize = true;
            this.enabledSubDeviceCheckBox.Location = new System.Drawing.Point(8, 52);
            this.enabledSubDeviceCheckBox.Name = "enabledSubDeviceCheckBox";
            this.enabledSubDeviceCheckBox.Size = new System.Drawing.Size(166, 16);
            this.enabledSubDeviceCheckBox.TabIndex = 21;
            this.enabledSubDeviceCheckBox.Text = "サブ再生デバイスを有効にする";
            this.enabledSubDeviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // subDeviceComboBox
            // 
            this.subDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subDeviceComboBox.FormattingEnabled = true;
            this.subDeviceComboBox.Location = new System.Drawing.Point(105, 70);
            this.subDeviceComboBox.Name = "subDeviceComboBox";
            this.subDeviceComboBox.Size = new System.Drawing.Size(306, 20);
            this.subDeviceComboBox.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "サブ再生デバイス";
            // 
            // mainDeviceComboBox
            // 
            this.mainDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mainDeviceComboBox.FormattingEnabled = true;
            this.mainDeviceComboBox.Location = new System.Drawing.Point(105, 18);
            this.mainDeviceComboBox.Name = "mainDeviceComboBox";
            this.mainDeviceComboBox.Size = new System.Drawing.Size(306, 20);
            this.mainDeviceComboBox.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "メイン再生デバイス";
            // 
            // TTSYukkuriConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saiseiDeviceGroupBox);
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.ttsSettingsGroupBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ttsShuruiComboBox);
            this.Controls.Add(this.label1);
            this.Name = "TTSYukkuriConfigPanel";
            this.Size = new System.Drawing.Size(1201, 638);
            this.Load += new System.EventHandler(this.TTSYukkuriConfigPanel_Load);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.saiseiDeviceGroupBox.ResumeLayout(false);
            this.saiseiDeviceGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ttsShuruiComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox ttsSettingsGroupBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.CheckBox enableWatchTPCheckBox;
        private System.Windows.Forms.CheckBox enableWatchMPCheckBox;
        private System.Windows.Forms.CheckBox enableWatchHPCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TPThresholdTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MPThresholdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox HPThresholdTextBox;
        private System.Windows.Forms.GroupBox saiseiDeviceGroupBox;
        private System.Windows.Forms.CheckBox enabledSubDeviceCheckBox;
        private System.Windows.Forms.ComboBox subDeviceComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox mainDeviceComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tpTextToSpeakTextBox;
        private System.Windows.Forms.TextBox mpTextToSpeakTextBox;
        private System.Windows.Forms.TextBox hpTextToSpeakTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox enabledSelfCheckBox;
    }
}
