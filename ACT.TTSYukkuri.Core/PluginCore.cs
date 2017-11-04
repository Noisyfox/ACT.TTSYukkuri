using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using ACT.TTSYukkuri.TTSServer;
using Advanced_Combat_Tracker;
using FFXIV.Framework.Common;
using NLog;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class PluginCore
    {
        #region Singleton

        private static PluginCore instance;

        public static PluginCore Instance =>
            instance ?? (instance = new PluginCore());

        private PluginCore()
        {
        }

        public static void Free()
        {
            instance = null;
        }

        #endregion Singleton

        #region Logger

        private Logger Logger => AppLog.DefaultLogger;

        #endregion Logger

        public TTSYukkuriConfigPanel ConfigPanel { get; private set; }

        public string PluginDirectory { get; private set; }

        private Label PluginStatusLabel;

        #region Replace TTS Method

        private FormActMain.PlayTtsDelegate originalTTSMethod;
        private FormActMain.PlaySoundDelegate originalSoundMethod;

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
                    if (this.replaceTTSMethodTimer.Enabled)
                    {
                        this.ReplaceTTSMethod();
                    }
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

            // サウンド再生メソッドを置き換える
            if (ActGlobals.oFormActMain.PlaySoundMethod != this.PlaySound)
            {
                this.originalSoundMethod = (FormActMain.PlaySoundDelegate)ActGlobals.oFormActMain.PlaySoundMethod.Clone();
                ActGlobals.oFormActMain.PlaySoundMethod = this.PlaySound;
            }
        }

        private void RestoreTTSMethod()
        {
            // 置き換えたTTSメソッドを元に戻す
            if (this.originalTTSMethod != null)
            {
                ActGlobals.oFormActMain.PlayTtsMethod = this.originalTTSMethod;
            }

            // 置き換えたサウンド再生メソッドを元に戻す
            if (this.originalSoundMethod != null)
            {
                ActGlobals.oFormActMain.PlaySoundMethod = this.originalSoundMethod;
            }
        }

        #endregion Replace TTS Method

        public void PlaySound(
            string wave,
            int volume)
        {
            if (!File.Exists(wave))
            {
                return;
            }

            SoundPlayerWrapper.Play(wave);
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

            SoundPlayerWrapper.Play(wave);
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
                this.Logger.Error(ex, "SpeakTTS で例外が発生しました。");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InitPlugin(
            IActPluginV1 plugin,
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            AppLog.LoadConfiguration(AppLog.HojoringConfig);

            try
            {
                this.PluginStatusLabel = pluginStatusText;
                pluginScreenSpace.Text = "YUKKURI";

                var pluginInfo = ActGlobals.oFormActMain.PluginGetSelfData(plugin);
                if (pluginInfo != null)
                {
                    this.PluginDirectory = pluginInfo.pluginFile.DirectoryName;
                }

                // 設定ファイルを読み込む
                TTSYukkuriConfig.Default.Load();

                // TTSのキャッシュを移行する
                this.MigrateTTSCache();

                // HojoringのSplashを表示する
                WPFHelper.Start();
                UpdateChecker.ShowSplash();

                // TTSサーバを開始する
                TTSServerController.Start();

                // 漢字変換を初期化する
                KanjiTranslator.Default.Initialize();

                // TTSを初期化する
                SpeechController.Default.Initialize();

                // FF14監視スレッドを初期化する
                FFXIVWatcher.Initialize();

                // 設定Panelを追加する
                pluginScreenSpace.Controls.Add(
                    this.ConfigPanel = new TTSYukkuriConfigPanel()
                    {
                        Dock = DockStyle.Fill
                    });

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

                PluginStatusLabel.Text = "Plugin Started";
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "InitPlugin error.");

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

        public void DeInitPlugin()
        {
            try
            {
                // TTSアクションを元に戻す
                this.StopReplaceTTSMethodTimer();
                this.RestoreTTSMethod();

                // TTSコントローラを開放する
                SpeechController.Default.Free();

                // Discordを終了する
                DiscordClientModel.Instance.Dispose();

                // TTSサーバを終了する
                TTSServerController.End();

                // FF14監視スレッドを開放する
                FFXIVWatcher.Deinitialize();

                // 漢字変換オブジェクトを開放する
                KanjiTranslator.Default.Dispose();

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

                // 設定を保存する
                TTSYukkuriConfig.Default.Save();

                this.PluginStatusLabel.Text = "Plugin Exited";
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "DeInitPlugin error.");
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
        /// 最終リリースのURL
        /// </summary>
        public const string LastestReleaseUrl = @"https://github.com/anoyetta/ACT.TTSYukkuri/releases/latest";

        /// <summary>
        /// アップデートを行う
        /// </summary>
        private void Update()
        {
            if ((DateTime.Now - TTSYukkuriConfig.Default.LastUpdateDateTime).TotalHours
                >= TTSYukkuriConfig.UpdateCheckInterval)
            {
                var message = UpdateChecker.Update(
                    "ACT.TTSYukkuri",
                    LastestReleaseUrl,
                    Assembly.GetExecutingAssembly());
                if (!string.IsNullOrWhiteSpace(message))
                {
                    this.Logger.Error(message);
                }

                TTSYukkuriConfig.Default.LastUpdateDateTime = DateTime.Now;
                TTSYukkuriConfig.Default.Save();
            }
        }
    }
}
