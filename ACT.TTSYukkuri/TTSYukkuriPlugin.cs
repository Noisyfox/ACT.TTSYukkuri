namespace ACT.TTSYukkuri
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class TTSYukkuriPlugin : IActPluginV1
    {
        private Label lblStatus;
        private byte[] originalTTS;
        private IntPtr ACT_TTSMethod;

        #region IActPluginV1 Members

        public void InitPlugin(
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            try
            {
                pluginScreenSpace.Text = "TTSゆっくり";

                // TTSを初期化する
                TTSYukkuriConfig.Default.Load();
                SpeechController.Default.Initialize();

                // 設定Panelを追加する
                var configPanel = new TTSYukkuriConfigPanel();
                configPanel.Dock = DockStyle.Fill;
                pluginScreenSpace.Controls.Add(configPanel);

                // Hand the status label's reference to our local var
                lblStatus = pluginStatusText;

                // Create some sort of parsing event handler. After the "+=" hit TAB twice and the code will be generated for you.
                IntPtr new_TTSMethod = Replacer.GetFunctionPointer(typeof(FormActMain_newTTS).GetMethod("newTTS", BindingFlags.Instance | BindingFlags.Public).MethodHandle);
                ACT_TTSMethod = Replacer.GetFunctionPointer(typeof(FormActMain).GetMethod("TTS", BindingFlags.Instance | BindingFlags.Public).MethodHandle);
                Replacer.InsertJumpToFunction(ACT_TTSMethod, new_TTSMethod, out originalTTS);

                lblStatus.Text = "Plugin Started";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    pluginScreenSpace,
                    "プラグインの初期化中に例外が発生しました。環境を確認してください" + Environment.NewLine + Environment.NewLine +
                    ex.ToString(),
                    "TTSゆっくりプラグイン",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        public void DeInitPlugin()
        {
            // Unsubscribe from any events you listen to when exiting!
            Replacer.RestoreFunction(ACT_TTSMethod, originalTTS);

            // 設定を保存する
            TTSYukkuriConfig.Default.Save();

            lblStatus.Text = "Plugin Exited";
        }

        #endregion
    }
}
