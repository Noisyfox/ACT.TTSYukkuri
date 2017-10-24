using System;
using System.IO;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ACT.TTSYukkuri.Config;
using Advanced_Combat_Tracker;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using RucheHome.Voiceroid;

namespace ACT.TTSYukkuri.Voiceroid
{
    /// <summary>
    /// VOICEROIDスピーチコントローラ
    /// </summary>
    public class VoiceroidSpeechController :
        ISpeechController
    {
        private VoiceroidConfig Config => TTSYukkuriConfig.Default.VoiceroidSettings;

        public ProcessFactory ProcessFactory { get; private set; }

        private CompositeDisposable CompositeDisposable { get; set; }

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
            lock (this)
            {
                if (this.CompositeDisposable != null)
                {
                    return;
                }

                this.CompositeDisposable = new CompositeDisposable();
            }

            this.ProcessFactory = new ProcessFactory()
                .AddTo(this.CompositeDisposable);

            // プロセス更新タイマ設定＆開始
            var updateTimer = new ReactiveTimer(TimeSpan.FromMilliseconds(100))
                .AddTo(this.CompositeDisposable);
            updateTimer
                .Subscribe(async x =>
                {
                    if (this.ProcessFactory == null)
                    {
                        return;
                    }

                    await this.ProcessFactory.Update();
                    foreach (var innerProcess in this.ProcessFactory.Processes)
                    {
                        var process = this.Config.Get(innerProcess.Id);
                        if (process != null)
                        {
                            if (!string.IsNullOrEmpty(innerProcess.ExecutablePath) &&
                                File.Exists(innerProcess.ExecutablePath))
                            {
                                process.Path = innerProcess.ExecutablePath;
                            }
                        }
                    }
                })
                .AddTo(this.CompositeDisposable);
            updateTimer.Start();
        }

        /// <summary>
        /// 開放する
        /// </summary>
        public async void Free()
        {
            if (this.Config.ExitVoiceroidWhenExit)
            {
                var process = this.Config.GetSelected()?.InnerProcess;
                await process?.Exit();
            }

            lock (this)
            {
                this.CompositeDisposable?.Dispose();
                this.CompositeDisposable = null;
                this.ProcessFactory = null;
            }
        }

        /// <summary>
        /// 指定されたVOICEROIDを起動する
        /// </summary>
        /// <returns>
        /// エラーメッセージ</returns>
        public async Task<string> Start()
        {
            var process = this.Config.GetSelected();
            if (process != null &&
                !process.InnerProcess.IsRunning)
            {
                if (!string.IsNullOrEmpty(process.Path) &&
                    File.Exists(process.Path))
                {
                    await process.InnerProcess.Run(process.Path);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public async void Speak(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            // 起動していなければ起動させる
            var err = await this.Start();
            if (!string.IsNullOrEmpty(err))
            {
                ActGlobals.oFormActMain.WriteInfoLog(
                    $"VOICEROID Speak error : {err}");
                return;
            }

#if false
            // 現在の条件をハッシュ化してWAVEファイル名を作る
            var wave = this.GetCacheFileName(
                TTSYukkuriConfig.Default.TTS,
                text,
                this.Config.ToString());

            if (!File.Exists(wave))
            {
                // 音声waveファイルを生成する
                var process = this.ProcessFactory?.Get(this.Config.VoiceroidId);
                if (process != null)
                {
                    if (await process.SetTalkText(text))
                    {
                        var result = await process.Save(wave);
                        if (!result.IsSucceeded)
                        {
                            ActGlobals.oFormActMain.WriteInfoLog(
                                $"VOICEROID Speak error : {result.Error}, {result.ExtraMessage}");
                            return;
                        }
                    }
                }
            }

            // 再生する
            SoundPlayerWrapper.Play(wave);
#else
            // 直接再生する
            var process = this.Config.GetSelected()?.InnerProcess;
            if (process != null)
            {
                if (await process.SetTalkText(text))
                {
                    if (!await process.Play())
                    {
                        ActGlobals.oFormActMain.WriteInfoLog(
                            $"VOICEROID Speak error");
                        return;
                    }
                }
            }
#endif
        }

        public static class NativeMethods
        {
            public enum ShowWindowCommands : uint
            {
                /// <summary>
                ///        Hides the window and activates another window.
                /// </summary>
                SW_HIDE = 0,

                /// <summary>
                ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
                /// </summary>
                SW_SHOWNORMAL = 1,

                /// <summary>
                ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
                /// </summary>
                SW_NORMAL = 1,

                /// <summary>
                ///        Activates the window and displays it as a minimized window.
                /// </summary>
                SW_SHOWMINIMIZED = 2,

                /// <summary>
                ///        Activates the window and displays it as a maximized window.
                /// </summary>
                SW_SHOWMAXIMIZED = 3,

                /// <summary>
                ///        Maximizes the specified window.
                /// </summary>
                SW_MAXIMIZE = 3,

                /// <summary>
                ///        Displays a window in its most recent size and position. This value is similar to <see cref="ShowWindowCommands.SW_SHOWNORMAL"/>, except the window is not activated.
                /// </summary>
                SW_SHOWNOACTIVATE = 4,

                /// <summary>
                ///        Activates the window and displays it in its current size and position.
                /// </summary>
                SW_SHOW = 5,

                /// <summary>
                ///        Minimizes the specified window and activates the next top-level window in the z-order.
                /// </summary>
                SW_MINIMIZE = 6,

                /// <summary>
                ///        Displays the window as a minimized window. This value is similar to <see cref="ShowWindowCommands.SW_SHOWMINIMIZED"/>, except the window is not activated.
                /// </summary>
                SW_SHOWMINNOACTIVE = 7,

                /// <summary>
                ///        Displays the window in its current size and position. This value is similar to <see cref="ShowWindowCommands.SW_SHOW"/>, except the window is not activated.
                /// </summary>
                SW_SHOWNA = 8,

                /// <summary>
                ///        Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
                /// </summary>
                SW_RESTORE = 9
            }

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
        }
    }
}
