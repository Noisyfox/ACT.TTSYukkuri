namespace ACT.TTSYukkuri.OpenJTalk
{
    partial class OpenJTalkSettingsPanel
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
            this.VoiceComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GainTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.VolumeTrackBar = new System.Windows.Forms.TrackBar();
            this.SpeedTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ToneTrackBar = new System.Windows.Forms.TrackBar();
            this.GainTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.VolumeTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SpeedTextBox = new System.Windows.Forms.TextBox();
            this.ToneTextBox = new System.Windows.Forms.TextBox();
            this.DefaultButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToneTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // VoiceComboBox
            // 
            this.VoiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VoiceComboBox.FormattingEnabled = true;
            this.VoiceComboBox.Location = new System.Drawing.Point(43, 22);
            this.VoiceComboBox.Name = "VoiceComboBox";
            this.VoiceComboBox.Size = new System.Drawing.Size(267, 20);
            this.VoiceComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Voice";
            // 
            // GainTrackBar
            // 
            this.GainTrackBar.LargeChange = 20;
            this.GainTrackBar.Location = new System.Drawing.Point(16, 57);
            this.GainTrackBar.Maximum = 400;
            this.GainTrackBar.Name = "GainTrackBar";
            this.GainTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.GainTrackBar.Size = new System.Drawing.Size(45, 167);
            this.GainTrackBar.TabIndex = 2;
            this.GainTrackBar.TickFrequency = 20;
            this.GainTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.GainTrackBar.Value = 200;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Gain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vol";
            // 
            // VolumeTrackBar
            // 
            this.VolumeTrackBar.LargeChange = 10;
            this.VolumeTrackBar.Location = new System.Drawing.Point(67, 57);
            this.VolumeTrackBar.Maximum = 100;
            this.VolumeTrackBar.Name = "VolumeTrackBar";
            this.VolumeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.VolumeTrackBar.Size = new System.Drawing.Size(45, 167);
            this.VolumeTrackBar.TabIndex = 5;
            this.VolumeTrackBar.TickFrequency = 10;
            this.VolumeTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.VolumeTrackBar.Value = 100;
            // 
            // SpeedTrackBar
            // 
            this.SpeedTrackBar.LargeChange = 20;
            this.SpeedTrackBar.Location = new System.Drawing.Point(118, 57);
            this.SpeedTrackBar.Maximum = 200;
            this.SpeedTrackBar.Minimum = 50;
            this.SpeedTrackBar.Name = "SpeedTrackBar";
            this.SpeedTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.SpeedTrackBar.Size = new System.Drawing.Size(45, 167);
            this.SpeedTrackBar.TabIndex = 6;
            this.SpeedTrackBar.TickFrequency = 10;
            this.SpeedTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.SpeedTrackBar.Value = 100;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Speed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "Tone";
            // 
            // ToneTrackBar
            // 
            this.ToneTrackBar.LargeChange = 10;
            this.ToneTrackBar.Location = new System.Drawing.Point(169, 57);
            this.ToneTrackBar.Maximum = 80;
            this.ToneTrackBar.Minimum = -80;
            this.ToneTrackBar.Name = "ToneTrackBar";
            this.ToneTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ToneTrackBar.Size = new System.Drawing.Size(45, 167);
            this.ToneTrackBar.TabIndex = 9;
            this.ToneTrackBar.TickFrequency = 10;
            this.ToneTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.ToneTrackBar.Value = 50;
            // 
            // GainTextBox
            // 
            this.GainTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.GainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GainTextBox.Location = new System.Drawing.Point(275, 68);
            this.GainTextBox.Name = "GainTextBox";
            this.GainTextBox.Size = new System.Drawing.Size(25, 12);
            this.GainTextBox.TabIndex = 10;
            this.GainTextBox.TabStop = false;
            this.GainTextBox.Text = "100";
            this.GainTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(241, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "Gain";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(226, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "Volume";
            // 
            // VolumeTextBox
            // 
            this.VolumeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.VolumeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VolumeTextBox.Location = new System.Drawing.Point(275, 86);
            this.VolumeTextBox.Name = "VolumeTextBox";
            this.VolumeTextBox.Size = new System.Drawing.Size(25, 12);
            this.VolumeTextBox.TabIndex = 13;
            this.VolumeTextBox.TabStop = false;
            this.VolumeTextBox.Text = "100";
            this.VolumeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(233, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "Speed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(239, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "Tone";
            // 
            // SpeedTextBox
            // 
            this.SpeedTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.SpeedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpeedTextBox.Location = new System.Drawing.Point(275, 104);
            this.SpeedTextBox.Name = "SpeedTextBox";
            this.SpeedTextBox.Size = new System.Drawing.Size(25, 12);
            this.SpeedTextBox.TabIndex = 16;
            this.SpeedTextBox.TabStop = false;
            this.SpeedTextBox.Text = "100";
            this.SpeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ToneTextBox
            // 
            this.ToneTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.ToneTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToneTextBox.Location = new System.Drawing.Point(275, 122);
            this.ToneTextBox.Name = "ToneTextBox";
            this.ToneTextBox.Size = new System.Drawing.Size(25, 12);
            this.ToneTextBox.TabIndex = 17;
            this.ToneTextBox.TabStop = false;
            this.ToneTextBox.Text = "100";
            this.ToneTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // DefaultButton
            // 
            this.DefaultButton.Location = new System.Drawing.Point(242, 222);
            this.DefaultButton.Name = "DefaultButton";
            this.DefaultButton.Size = new System.Drawing.Size(75, 23);
            this.DefaultButton.TabIndex = 18;
            this.DefaultButton.Text = "default";
            this.DefaultButton.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(306, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(306, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "%";
            // 
            // OpenJTalkSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.DefaultButton);
            this.Controls.Add(this.ToneTextBox);
            this.Controls.Add(this.SpeedTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.VolumeTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.GainTextBox);
            this.Controls.Add(this.ToneTrackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SpeedTrackBar);
            this.Controls.Add(this.VolumeTrackBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GainTrackBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VoiceComboBox);
            this.Name = "OpenJTalkSettingsPanel";
            this.Size = new System.Drawing.Size(334, 263);
            ((System.ComponentModel.ISupportInitialize)(this.GainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToneTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox VoiceComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar GainTrackBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar VolumeTrackBar;
        private System.Windows.Forms.TrackBar SpeedTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar ToneTrackBar;
        private System.Windows.Forms.TextBox GainTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox VolumeTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SpeedTextBox;
        private System.Windows.Forms.TextBox ToneTextBox;
        private System.Windows.Forms.Button DefaultButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}
