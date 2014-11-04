namespace ACT.TTSYukkuri.Yukkuri
{
    partial class YukkuriSettingsPanel
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
            this.enabledYukkuriVolumeSettingCheckBox = new System.Windows.Forms.CheckBox();
            this.yukkuriVolumeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.yukkuriSpeedTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mainDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.subDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.enabledSubDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // enabledYukkuriVolumeSettingCheckBox
            // 
            this.enabledYukkuriVolumeSettingCheckBox.AllowDrop = true;
            this.enabledYukkuriVolumeSettingCheckBox.AutoSize = true;
            this.enabledYukkuriVolumeSettingCheckBox.Location = new System.Drawing.Point(3, 48);
            this.enabledYukkuriVolumeSettingCheckBox.Name = "enabledYukkuriVolumeSettingCheckBox";
            this.enabledYukkuriVolumeSettingCheckBox.Size = new System.Drawing.Size(175, 16);
            this.enabledYukkuriVolumeSettingCheckBox.TabIndex = 0;
            this.enabledYukkuriVolumeSettingCheckBox.Text = "ゆっくりの音量調節を有効にする";
            this.enabledYukkuriVolumeSettingCheckBox.UseVisualStyleBackColor = true;
            // 
            // yukkuriVolumeTextBox
            // 
            this.yukkuriVolumeTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.yukkuriVolumeTextBox.Location = new System.Drawing.Point(44, 77);
            this.yukkuriVolumeTextBox.MaxLength = 3;
            this.yukkuriVolumeTextBox.Name = "yukkuriVolumeTextBox";
            this.yukkuriVolumeTextBox.Size = new System.Drawing.Size(59, 19);
            this.yukkuriVolumeTextBox.TabIndex = 1;
            this.yukkuriVolumeTextBox.Text = "0";
            this.yukkuriVolumeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-2, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "音量";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "(0～100)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "(50～300) 100近辺が標準値です";
            // 
            // yukkuriSpeedTextBox
            // 
            this.yukkuriSpeedTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.yukkuriSpeedTextBox.Location = new System.Drawing.Point(44, 14);
            this.yukkuriSpeedTextBox.MaxLength = 3;
            this.yukkuriSpeedTextBox.Name = "yukkuriSpeedTextBox";
            this.yukkuriSpeedTextBox.Size = new System.Drawing.Size(59, 19);
            this.yukkuriSpeedTextBox.TabIndex = 0;
            this.yukkuriSpeedTextBox.Text = "0";
            this.yukkuriSpeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-2, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "速度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-2, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "メイン再生デバイス";
            // 
            // mainDeviceComboBox
            // 
            this.mainDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mainDeviceComboBox.FormattingEnabled = true;
            this.mainDeviceComboBox.Location = new System.Drawing.Point(97, 123);
            this.mainDeviceComboBox.Name = "mainDeviceComboBox";
            this.mainDeviceComboBox.Size = new System.Drawing.Size(306, 20);
            this.mainDeviceComboBox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-2, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "サブ再生デバイス";
            // 
            // subDeviceComboBox
            // 
            this.subDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subDeviceComboBox.FormattingEnabled = true;
            this.subDeviceComboBox.Location = new System.Drawing.Point(97, 177);
            this.subDeviceComboBox.Name = "subDeviceComboBox";
            this.subDeviceComboBox.Size = new System.Drawing.Size(306, 20);
            this.subDeviceComboBox.TabIndex = 15;
            // 
            // enabledSubDeviceCheckBox
            // 
            this.enabledSubDeviceCheckBox.AutoSize = true;
            this.enabledSubDeviceCheckBox.Location = new System.Drawing.Point(3, 155);
            this.enabledSubDeviceCheckBox.Name = "enabledSubDeviceCheckBox";
            this.enabledSubDeviceCheckBox.Size = new System.Drawing.Size(166, 16);
            this.enabledSubDeviceCheckBox.TabIndex = 16;
            this.enabledSubDeviceCheckBox.Text = "サブ再生デバイスを有効にする";
            this.enabledSubDeviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // YukkuriSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enabledSubDeviceCheckBox);
            this.Controls.Add(this.subDeviceComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mainDeviceComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.yukkuriSpeedTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yukkuriVolumeTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.enabledYukkuriVolumeSettingCheckBox);
            this.Name = "YukkuriSettingsPanel";
            this.Size = new System.Drawing.Size(406, 296);
            this.Load += new System.EventHandler(this.YukkuriSettingsPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enabledYukkuriVolumeSettingCheckBox;
        private System.Windows.Forms.TextBox yukkuriVolumeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox yukkuriSpeedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox mainDeviceComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox subDeviceComboBox;
        private System.Windows.Forms.CheckBox enabledSubDeviceCheckBox;
    }
}
