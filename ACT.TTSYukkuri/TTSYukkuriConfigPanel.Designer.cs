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
            this.label1 = new System.Windows.Forms.Label();
            this.ttsShuruiComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ttsSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.enableWatchHPCheckBox = new System.Windows.Forms.CheckBox();
            this.enableWatchMPCheckBox = new System.Windows.Forms.CheckBox();
            this.enableWatchTPCheckBox = new System.Windows.Forms.CheckBox();
            this.enableWatchDebuffCheckBox = new System.Windows.Forms.CheckBox();
            this.HPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TPThresholdTextBox = new System.Windows.Forms.TextBox();
            this.optionGroupBox.SuspendLayout();
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
            this.ttsShuruiComboBox.TabIndex = 1;
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
            this.ttsSettingsGroupBox.Location = new System.Drawing.Point(5, 93);
            this.ttsSettingsGroupBox.Name = "ttsSettingsGroupBox";
            this.ttsSettingsGroupBox.Size = new System.Drawing.Size(611, 538);
            this.ttsSettingsGroupBox.TabIndex = 4;
            this.ttsSettingsGroupBox.TabStop = false;
            this.ttsSettingsGroupBox.Text = "TTSの設定";
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Controls.Add(this.label5);
            this.optionGroupBox.Controls.Add(this.TPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.label4);
            this.optionGroupBox.Controls.Add(this.MPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.label2);
            this.optionGroupBox.Controls.Add(this.HPThresholdTextBox);
            this.optionGroupBox.Controls.Add(this.enableWatchDebuffCheckBox);
            this.optionGroupBox.Controls.Add(this.enableWatchTPCheckBox);
            this.optionGroupBox.Controls.Add(this.enableWatchMPCheckBox);
            this.optionGroupBox.Controls.Add(this.enableWatchHPCheckBox);
            this.optionGroupBox.Location = new System.Drawing.Point(622, 93);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.Size = new System.Drawing.Size(720, 538);
            this.optionGroupBox.TabIndex = 5;
            this.optionGroupBox.TabStop = false;
            this.optionGroupBox.Text = "オプション設定";
            // 
            // enableWatchHPCheckBox
            // 
            this.enableWatchHPCheckBox.AutoSize = true;
            this.enableWatchHPCheckBox.Location = new System.Drawing.Point(6, 33);
            this.enableWatchHPCheckBox.Name = "enableWatchHPCheckBox";
            this.enableWatchHPCheckBox.Size = new System.Drawing.Size(152, 16);
            this.enableWatchHPCheckBox.TabIndex = 0;
            this.enableWatchHPCheckBox.Text = "PartyのHP低下を監視する";
            this.enableWatchHPCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableWatchMPCheckBox
            // 
            this.enableWatchMPCheckBox.AutoSize = true;
            this.enableWatchMPCheckBox.Location = new System.Drawing.Point(6, 108);
            this.enableWatchMPCheckBox.Name = "enableWatchMPCheckBox";
            this.enableWatchMPCheckBox.Size = new System.Drawing.Size(153, 16);
            this.enableWatchMPCheckBox.TabIndex = 1;
            this.enableWatchMPCheckBox.Text = "PartyのMP低下を監視する";
            this.enableWatchMPCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableWatchTPCheckBox
            // 
            this.enableWatchTPCheckBox.AutoSize = true;
            this.enableWatchTPCheckBox.Location = new System.Drawing.Point(6, 188);
            this.enableWatchTPCheckBox.Name = "enableWatchTPCheckBox";
            this.enableWatchTPCheckBox.Size = new System.Drawing.Size(151, 16);
            this.enableWatchTPCheckBox.TabIndex = 2;
            this.enableWatchTPCheckBox.Text = "PartyのTP低下を監視する";
            this.enableWatchTPCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableWatchDebuffCheckBox
            // 
            this.enableWatchDebuffCheckBox.AutoSize = true;
            this.enableWatchDebuffCheckBox.Location = new System.Drawing.Point(6, 271);
            this.enableWatchDebuffCheckBox.Name = "enableWatchDebuffCheckBox";
            this.enableWatchDebuffCheckBox.Size = new System.Drawing.Size(141, 16);
            this.enableWatchDebuffCheckBox.TabIndex = 3;
            this.enableWatchDebuffCheckBox.Text = "Partyのデバフを監視する";
            this.enableWatchDebuffCheckBox.UseVisualStyleBackColor = true;
            // 
            // HPThresholdTextBox
            // 
            this.HPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.HPThresholdTextBox.Location = new System.Drawing.Point(6, 55);
            this.HPThresholdTextBox.MaxLength = 3;
            this.HPThresholdTextBox.Name = "HPThresholdTextBox";
            this.HPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.HPThresholdTextBox.TabIndex = 4;
            this.HPThresholdTextBox.Text = "0";
            this.HPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "%以下になったら読上げる";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "%以下になったら読上げる";
            // 
            // MPThresholdTextBox
            // 
            this.MPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MPThresholdTextBox.Location = new System.Drawing.Point(6, 130);
            this.MPThresholdTextBox.MaxLength = 3;
            this.MPThresholdTextBox.Name = "MPThresholdTextBox";
            this.MPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.MPThresholdTextBox.TabIndex = 6;
            this.MPThresholdTextBox.Text = "0";
            this.MPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "%以下になったら読上げる";
            // 
            // TPThresholdTextBox
            // 
            this.TPThresholdTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TPThresholdTextBox.Location = new System.Drawing.Point(6, 210);
            this.TPThresholdTextBox.MaxLength = 3;
            this.TPThresholdTextBox.Name = "TPThresholdTextBox";
            this.TPThresholdTextBox.Size = new System.Drawing.Size(59, 19);
            this.TPThresholdTextBox.TabIndex = 8;
            this.TPThresholdTextBox.Text = "0";
            this.TPThresholdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TTSYukkuriConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.ttsSettingsGroupBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ttsShuruiComboBox);
            this.Controls.Add(this.label1);
            this.Name = "TTSYukkuriConfigPanel";
            this.Size = new System.Drawing.Size(1345, 638);
            this.Load += new System.EventHandler(this.TTSYukkuriConfigPanel_Load);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ttsShuruiComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox ttsSettingsGroupBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.CheckBox enableWatchDebuffCheckBox;
        private System.Windows.Forms.CheckBox enableWatchTPCheckBox;
        private System.Windows.Forms.CheckBox enableWatchMPCheckBox;
        private System.Windows.Forms.CheckBox enableWatchHPCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TPThresholdTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MPThresholdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox HPThresholdTextBox;
    }
}
