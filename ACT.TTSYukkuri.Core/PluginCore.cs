using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using ACT.TTSYukkuri.SoundPlayer;
using ACT.TTSYukkuri.TTSServer;
using Advanced_Combat_Tracker;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class PluginCore
    {
        #region Singleton

        private static PluginCore instance = new PluginCore();

        public static PluginCore Instance => instance;

        private PluginCore()
        {
        }

        #endregion Singleton

        public TTSYukkuriConfigPanel ConfigPanel { get; private set; }

        public string PluginDirectory { get; private set; }

        private Label lblStatus;

        #region Replace TTS Method

        private FormActMain.PlayTtsDelegate originalTTSMethod;

        private System.Timers.Timer replaceTTSMethodTimer;

        private void StopReplaceTTSMethodTimer()
        {
            // タイマを止める
            if (this.replaceTTSMethodTimer.Enabled)
            {
                this.replaceTTSMethodTimer.Stop();
                this.replaceTTSMethodTimer.Dispose();
                this.replaceTTSMethodTimer = null;
            }
        }

        private void StartReplaceTTSMethodTimer()
        {
            this.replaceTTSMethodTimer = new System.Timers.Timer()
            {
                Interval = 3 * 1000,
                AutoReset = true,
            };

            // 置き換え監視タイマを開始する
            if (!this.replaceTTSMethodTimer.Enabled)
            {
                this.replaceTTSMethodTimer.Elapsed += (s, e) =>
                {
                    this.ReplaceTTSMethod();
                };

                this.replaceTTSMethodTimer.Start();
            }
        }

        private void ReplaceTTSMethod()
        {
            // TTSメソッドを置き換える
            if (ActGlobals.oFormActMain.PlayTtsMethod != this.Speak)
            {
                this.originalTTSMethod = (FormActMain.PlayTtsDelegate)ActGlobals.oFormActMain.PlayTtsMethod.Clone();
                ActGlobals.oFormActMain.PlayTtsMethod = this.Speak;
            }
        }

        private void RestoreTTSMethod()
        {
            // 置き換えたTTSメソッドを元に戻す
            if (this.originalTTSMethod != null)
            {
                ActGlobals.oFormActMain.PlayTtsMethod = this.originalTTSMethod;
            }
        }

        #endregion Replace TTS Method

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

            // ファイルじゃない（TTS）？
            if (!textToSpeak.EndsWith(".wav"))
            {
                Task.Run(() =>
                {
                    this.SpeakTTS(textToSpeak);
                });

                return;
            }

            // waveファイルとして再生する
            Task.Run(() =>
            {
                var wave = textToSpeak;
                if (!File.Exists(wave))
                {
                    var dirs = new string[]
                    {
                        Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"resources\wav"),
                        Path.Combine(this.PluginDirectory, @"resources\wav"),
                    };

                    foreach (var dir in dirs)
                    {
                        var f = Path.Combine(dir, wave);
                        if (File.Exists(f))
                        {
                            wave = f;
                            break;
                        }
                    }
                }

                if (!File.Exists(wave))
                {
                    return;
                }

                if (TTSYukkuriConfig.Default.EnabledSubDevice)
                {
                    NAudioPlayer.Play(
                        TTSYukkuriConfig.Default.SubDeviceID,
                        wave,
                        false,
                        TTSYukkuriConfig.Default.WaveVolume);
                }

                NAudioPlayer.Play(
                    TTSYukkuriConfig.Default.MainDeviceID,
                    wave,
                    false,
                    TTSYukkuriConfig.Default.WaveVolume);
            });
        }

        /// <summary>
        /// TTSを読上げる
        /// </summary>
        /// <param name="textToSpeak">読上げるテキスト</param>
        private void SpeakTTS(
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

        public void DeInitPlugin()
        {
            try
            {
                // TTSアクションを元に戻す
                this.StopReplaceTTSMethodTimer();
                this.RestoreTTSMethod();

                // Discordを終了する
                DiscordClientModel.Instance.Dispose();

                // TTSサーバを終了する
                TTSServerController.End();

                // FF14監視スレッドを開放する
                FF14Watcher.Deinitialize();

                // 漢字変換オブジェクトを開放する
                KanjiTranslator.Default.Dispose();

                // 設定を保存する
                TTSYukkuriConfig.Default.Save();

                // プレイヤを開放する
                NAudioPlayer.DisposePlayers();

                // TTS用waveファイルを削除する？
                if (TTSYukkuriConfig.Default.WaveCacheClearEnable)
                {
                    var appdir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"anoyetta\ACT\tts cache");

                    if (Directory.Exists(appdir))
                    {
                        foreach (var file in Directory.GetFiles(appdir, "*.wav"))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                this.lblStatus.Text = "Plugin Exited";
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "TTSゆっくりプラグインの終了時に例外が発生しました。");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InitPlugin(
            IActPluginV1 plugin,
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            try
            {
                this.lblStatus = pluginStatusText;
                pluginScreenSpace.Text = "TTSゆっくり";

                var pluginInfo = ActGlobals.oFormActMain.PluginGetSelfData(plugin);
                if (pluginInfo != null)
                {
                    this.PluginDirectory = pluginInfo.pluginFile.DirectoryName;
                }

                // TTSのキャッシュを移行する
                this.MigrateTTSCache();

                // TTSサーバを開始する
                TTSServerController.Start();

                // 漢字変換を初期化する
                KanjiTranslator.Default.Initialize();

                // TTSを初期化する
                TTSYukkuriConfig.Default.Load();
                SpeechController.Default.Initialize();

                // FF14監視スレッドを初期化する
                FF14Watcher.Initialize();

                // 設定Panelを追加する
                this.ConfigPanel = new TTSYukkuriConfigPanel();
                this.ConfigPanel.Dock = DockStyle.Fill;
                pluginScreenSpace.Controls.Add(ConfigPanel);

                // TTSメソッドを置き換える
                this.StartReplaceTTSMethodTimer();

                // Discordに接続する
                DiscordClientModel.Instance.Initialize();
                DiscordClientModel.Instance.Connect(true);

                // アップデートを確認する
                Task.Run(() =>
                {
                    this.Update();
                });

                lblStatus.Text = "Plugin Started";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ActGlobals.oFormActMain,
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

        /// <summary>
        /// TTSのキャッシュ(waveファイル)をマイグレーションする
        /// </summary>
        private void MigrateTTSCache()
        {
            var oldCacheDir = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                 @"anoyetta\ACT");

            var newCacheDir = Path.Combine(
                oldCacheDir,
                "tts cache");

            if (!Directory.Exists(newCacheDir))
            {
                Directory.CreateDirectory(newCacheDir);
            }

            foreach (var file in Directory.EnumerateFiles(
                oldCacheDir, "*.wav", SearchOption.TopDirectoryOnly))
            {
                var dest = Path.Combine(
                    newCacheDir,
                    Path.GetFileName(file));

                if (File.Exists(dest))
                {
                    File.Delete(dest);
                }

                File.Move(file, dest);
            }
        }

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
