﻿namespace ACT.TTSYukkuri
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.SoundPlayer;
    using ACT.TTSYukkuri.TTSServer;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class TTSYukkuriPlugin :
        IActPluginV1
    {
        private Label lblStatus;

        private FormActMain.PlayTtsDelegate originalTTSDelegate;

        public static string PluginDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TTSYukkuriPlugin()
        {
            // このDLLの配置場所とACT標準のPluginディレクトリも解決の対象にする
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                try
                {
                    var asm = new AssemblyName(e.Name);

                    var plugin = ActGlobals.oFormActMain.PluginGetSelfData(this);
                    if (plugin != null)
                    {
                        var thisDirectory = plugin.pluginFile.DirectoryName;
                        var path1 = Path.Combine(thisDirectory, asm.Name + ".dll");
                        if (File.Exists(path1))
                        {
                            return Assembly.LoadFrom(path1);
                        }
                    }

                    var pluginDirectory = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Advanced Combat Tracker\Plugins");

                    var path = Path.Combine(pluginDirectory, asm.Name + ".dll");

                    if (File.Exists(path))
                    {
                        return Assembly.LoadFrom(path);
                    }
                }
                catch (Exception ex)
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(
                        ex,
                        "ACT.TTSYukkuri Assemblyの解決で例外が発生しました");
                }

                return null;
            };
        }

        /// <summary>
        /// 設定Panel（設定タブ）
        /// </summary>
        public static TTSYukkuriConfigPanel ConfigPanel
        {
            get;
            private set;
        }

        /// <summary>
        /// ACT本体の置き換え用のメソッド
        /// </summary>
        /// <param name="textToSpeak">読上げるテキスト</param>
        public void SpeakForActMain(
            string textToSpeak)
        {
            const string waitCommand = "/wait";

            try
            {
                // waitなし？
                if (!textToSpeak.StartsWith(waitCommand))
                {
                    SpeechController.Default.Speak(textToSpeak);
                }
                else
                {
                    var values = textToSpeak.Split(',');

                    // 分割できない？
                    if (values.Length < 2)
                    {
                        // 普通に読上げて終わる
                        SpeechController.Default.Speak(textToSpeak);
                        return;
                    }

                    var command = values[0].Trim();
                    var message = values[1].Trim();

                    // 秒数を取り出す
                    var delayAsText = command.Replace(waitCommand, string.Empty);
                    int delay = 0;
                    if (!int.TryParse(delayAsText, out delay))
                    {
                        // 普通に読上げて終わる
                        SpeechController.Default.Speak(textToSpeak);
                        return;
                    }

                    // ディレイをかけて読上げる
                    SpeechController.Default.SpeakWithDelay(
                        message,
                        delay);
                }
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "TTSYukkuri newTTSで例外が発生しました。");
            }
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

            Task.Run(() =>
            {
                // ファイルじゃない？
                if (!textToSpeak.EndsWith(".wav"))
                {
                    // 喋って終わる
                    SpeechController.Default.Speak(
                        textToSpeak);

                    return;
                }

                if (File.Exists(textToSpeak))
                {
                    ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                    {
                        if (TTSYukkuriConfig.Default.EnabledSubDevice)
                        {
                            NAudioPlayer.Play(
                                TTSYukkuriConfig.Default.SubDeviceNo,
                                textToSpeak,
                                false,
                                TTSYukkuriConfig.Default.WaveVolume);
                        }

                        NAudioPlayer.Play(
                            TTSYukkuriConfig.Default.MainDeviceNo,
                            textToSpeak,
                            false,
                            TTSYukkuriConfig.Default.WaveVolume);
                    });
                }
                else
                {
                    // ACTのパスを取得する
                    var baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var waveDir = Path.Combine(
                        baseDir,
                        @"resources\wav");

                    var wave = Path.Combine(waveDir, textToSpeak);
                    if (File.Exists(wave))
                    {
                        ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                        {
                            if (TTSYukkuriConfig.Default.EnabledSubDevice)
                            {
                                NAudioPlayer.Play(
                                    TTSYukkuriConfig.Default.SubDeviceNo,
                                    wave,
                                    false,
                                    TTSYukkuriConfig.Default.WaveVolume);
                            }

                            NAudioPlayer.Play(
                                TTSYukkuriConfig.Default.MainDeviceNo,
                                wave,
                                false,
                                TTSYukkuriConfig.Default.WaveVolume);
                        });
                    }
                }
            }).ContinueWith((t) =>
            {
                t.Dispose();
            });
        }

        #region IActPluginV1 Members

        public void InitPlugin(
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            try
            {
                pluginScreenSpace.Text = "TTSゆっくり";

                var plugin = ActGlobals.oFormActMain.PluginGetSelfData(this);
                if (plugin != null)
                {
                    TTSYukkuriPlugin.PluginDirectory = plugin.pluginFile.DirectoryName;
                }

                // 漢字変換を初期化する
                KanjiTranslator.Default.Initialize();

                // TTSサーバを開始する
                TTSServerController.Start();

                Application.ApplicationExit += (s, e) =>
                {
                    TTSServerController.End();
                };

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

                // TTSメソッドを置き換える
                this.originalTTSDelegate = (FormActMain.PlayTtsDelegate)ActGlobals.oFormActMain.PlayTtsMethod.Clone();
                ActGlobals.oFormActMain.PlayTtsMethod =
                    new FormActMain.PlayTtsDelegate(this.SpeakForActMain);

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
            // 置き換えたTTSメソッドを元に戻す
            if (this.originalTTSDelegate != null)
            {
                ActGlobals.oFormActMain.PlayTtsMethod = this.originalTTSDelegate;
            }

            // FF14監視スレッドを開放する
            FF14Watcher.Deinitialize();

            // 漢字変換オブジェクトを開放する
            KanjiTranslator.Default.Dispose();

            // TTSサーバを終了する
            TTSServerController.End();

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
