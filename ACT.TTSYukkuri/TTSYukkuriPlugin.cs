namespace ACT.TTSYukkuri
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.SoundPlayer;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class TTSYukkuriPlugin : IActPluginV1
    {
        private Label lblStatus;
        private byte[] originalTTS;
        private IntPtr ACT_TTSMethod;

        /// <summary>
        /// 設定Panel（設定タブ）
        /// </summary>
        public static TTSYukkuriConfigPanel ConfigPanel
        {
            get;
            private set;
        }

        /// <summary>
        /// テキストを読上げる
        /// </summary>
        /// <param name="textToSpeak">読上げるテキスト</param>
        public void Speak(
            string textToSpeak)
        {
            if (string.IsNullOrWhiteSpace(textToSpeak))
            {
                return;
            }

            // ファイルか？
            if (File.Exists(textToSpeak))
            {
                if (TTSYukkuriConfig.Default.EnabledSubDevice)
                {
                    NAudioPlayer.Play(
                        TTSYukkuriConfig.Default.SubDeviceNo,
                        textToSpeak,
                        false);
                }

                NAudioPlayer.Play(
                    TTSYukkuriConfig.Default.MainDeviceNo,
                    textToSpeak,
                    false);
            }
            else
            {
                SpeechController.Default.Speak(
                    textToSpeak);
            }
        }

        #region IActPluginV1 Members

        public void InitPlugin(
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            try
            {
                pluginScreenSpace.Text = "TTSゆっくり";

                // 漢字変換を初期化する
                KanjiTranslator.Default.Initialize();

                // TTSを初期化する
                TTSYukkuriConfig.Default.Load();
                SpeechController.Default.Initialize();

                // FF14監視スレッドを初期化する
                FF14Watcher.Initialize();

                // 設定Panelを追加する
                ConfigPanel = new TTSYukkuriConfigPanel();
                ConfigPanel.Dock = DockStyle.Fill;
                pluginScreenSpace.Controls.Add(ConfigPanel);

                // Hand the status label's reference to our local var
                lblStatus = pluginStatusText;

                // Create some sort of parsing event handler. After the "+=" hit TAB twice and the code will be generated for you.
                IntPtr new_TTSMethod = Replacer.GetFunctionPointer(typeof(FormActMain_newTTS).GetMethod("newTTS", BindingFlags.Instance | BindingFlags.Public).MethodHandle);
                ACT_TTSMethod = Replacer.GetFunctionPointer(typeof(FormActMain).GetMethod("TTS", BindingFlags.Instance | BindingFlags.Public).MethodHandle);
                Replacer.InsertJumpToFunction(ACT_TTSMethod, new_TTSMethod, out originalTTS);

                // アップデートを確認する
                this.Update();

                lblStatus.Text = "Plugin Started";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    pluginScreenSpace,
                    "プラグインの初期化中に例外が発生しました。環境を確認してACTを再起動して下さい" + Environment.NewLine + Environment.NewLine +
                    ex.ToString(),
                    "TTSゆっくりプラグイン",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                // TTSをゆっくりに戻す
                TTSYukkuriConfig.Default.TTS = TTSType.Yukkuri;
                TTSYukkuriConfig.Default.Save();
            }
        }

        public void DeInitPlugin()
        {
            // Unsubscribe from any events you listen to when exiting!
            Replacer.RestoreFunction(ACT_TTSMethod, originalTTS);

            // FF14監視スレッドを開放する
            FF14Watcher.Deinitialize();

            // 漢字変換オブジェクトを開放する
            KanjiTranslator.Default.Dispose();

            // 設定を保存する
            TTSYukkuriConfig.Default.Save();

            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        /// <summary>
        /// アップデートを行う
        /// </summary>
        private void Update()
        {
            if ((DateTime.Now - TTSYukkuriConfig.Default.LastUpdateDatetime).TotalHours > 6d)
            {
                var message = UpdateChecker.Update();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(
                        new Exception(),
                        message);
                }

                TTSYukkuriConfig.Default.LastUpdateDatetime = DateTime.Now;
                TTSYukkuriConfig.Default.Save();
            }
        }
    }
}
