using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Config.Views;
using ACT.TTSYukkuri.Discord.Models;
using ACT.TTSYukkuri.TTSServer;
using Advanced_Combat_Tracker;
using FFXIV.Framework.Bridge;
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

        #region Replace TTS Method

        private FormActMain.PlayTtsDelegate originalTTSMethod;
        private FormActMain.PlaySoundDelegate originalSoundMethod;

        private System.Timers.Timer replaceTTSMethodTimer;

        private void StopReplaceTTSMethodTimer()
        {
            // タイマを止める
            if (this.replaceTTSMethodTimer != null &&
                this.replaceTTSMethodTimer.Enabled)
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

        #region Play Method

        public void PlaySound(string wave, int volume) => this.PlaySound(wave, volume, PlayDevices.Both);

        public void PlaySound(
            string wave,
            int volume,
            PlayDevices playDevice = PlayDevices.Both)
        {
            if (!File.Exists(wave))
            {
                return;
            }

            Task.Run(() => SoundPlayerWrapper.Play(wave, playDevice));
        }

        public void Speak(string message) => this.Speak(message, PlayDevices.Both);

        public void Speak(
            string message,
            PlayDevices playDevice = PlayDevices.Both)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            // ファイルじゃない（TTS）？
            if (!message.EndsWith(".wav") &&
                !message.EndsWith(".wave"))
            {
                Task.Run(() => this.SpeakTTS(message, playDevice));
                return;
            }

            // waveファイルとして再生する
            var wave = message;
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

            // Volumeはダミーなので0で指定する
            this.PlaySound(wave, 0, playDevice);
        }

        private void SpeakTTS(
            string textToSpeak,
            PlayDevices playDevice = PlayDevices.Both)
        {
            const string waitCommand = "/wait";

            try
            {
                // waitなし？
                if (!textToSpeak.StartsWith(waitCommand))
                {
                    SpeechController.Default.Speak(textToSpeak, playDevice);
                }
                else
                {
                    var values = textToSpeak.Split(',');

                    // 分割できない？
                    if (values.Length < 2)
                    {
                        // 普通に読上げて終わる
                        SpeechController.Default.Speak(textToSpeak, playDevice);
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
                        SpeechController.Default.Speak(textToSpeak, playDevice);
                        return;
                    }

                    // ディレイをかけて読上げる
                    SpeechController.Default.SpeakWithDelay(
                        message,
                        delay,
                        playDevice);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "SpeakTTS で例外が発生しました。");
            }
        }

        #endregion Play Method

        public string PluginDirectory { get; private set; }

        private Label PluginStatusLabel;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InitPlugin(
            IActPluginV1 plugin,
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            AppLog.LoadConfiguration(AppLog.HojoringConfig);
            this.Logger.Trace(Assembly.GetExecutingAssembly().GetName().ToString() + " start.");

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
                Settings.Default.Load();

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
                pluginScreenSpace.Controls.Add(new ElementHost()
                {
                    Child = new ConfigBaseView(),
                    Dock = DockStyle.Fill,
                });

                // TTSメソッドを置き換える
                this.StartReplaceTTSMethodTimer();

                // Discordに接続する
                DiscordClientModel.Instance.Initialize();
                DiscordClientModel.Instance.Connect(true);

                // Bridgeにメソッドを登録する
                PlayBridge.Instance.PlayMainDeviceDelegate =
                    new PlayBridge.PlayMainDevice(message => this.Speak(message, PlayDevices.Main));
                PlayBridge.Instance.PlaySubDeviceDelegate =
                    new PlayBridge.PlaySubDevice(message => this.Speak(message, PlayDevices.Sub));

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
                Settings.Default.TTS = TTSType.Yukkuri;
                Settings.Default.Save();
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

                // Bridgeにメソッドを解除する
                PlayBridge.Instance.PlayMainDeviceDelegate = null;
                PlayBridge.Instance.PlaySubDeviceDelegate = null;

                // Discordを終了する
                DiscordClientModel.Instance.Dispose();

                // TTSサーバを終了する
                TTSServerController.End();

                // FF14監視スレッドを開放する
                FFXIVWatcher.Deinitialize();

                // 漢字変換オブジェクトを開放する
                KanjiTranslator.Default.Dispose();

                // TTS用waveファイルを削除する？
                if (Settings.Default.WaveCacheClearEnable)
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
                Settings.Default.Save();

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
            if ((DateTime.Now - Settings.Default.LastUpdateDateTime).TotalHours
                >= Settings.UpdateCheckInterval)
            {
                var message = UpdateChecker.Update(
                    "ACT.TTSYukkuri",
                    LastestReleaseUrl,
                    Assembly.GetExecutingAssembly());
                if (!string.IsNullOrWhiteSpace(message))
                {
                    this.Logger.Error(message);
                }

                Settings.Default.LastUpdateDateTime = DateTime.Now;
                Settings.Default.Save();
            }
        }
    }
}
