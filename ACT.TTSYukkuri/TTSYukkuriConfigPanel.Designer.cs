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
            this.ttsSettingsGroupBox.Size = new System.Drawing.Size(944, 538);
            this.ttsSettingsGroupBox.TabIndex = 4;
            this.ttsSettingsGroupBox.TabStop = false;
            this.ttsSettingsGroupBox.Text = "TTSの設定";
            // 
            // TTSYukkuriConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ttsSettingsGroupBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ttsShuruiComboBox);
            this.Controls.Add(this.label1);
            this.Name = "TTSYukkuriConfigPanel";
            this.Size = new System.Drawing.Size(952, 638);
            this.Load += new System.EventHandler(this.TTSYukkuriConfigPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ttsShuruiComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox ttsSettingsGroupBox;
    }
}
