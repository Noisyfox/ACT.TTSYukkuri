namespace ACT.TTSYukkuri
{
    using System;
    using System.Threading;

    using ACT.TTSYukkuri.Config;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// スピークdelegate
    /// </summary>
    /// <param name="textToSpeak"></param>
    public delegate void Speak(string textToSpeak);

    /// <summary>
    /// FF14を監視する
    /// </summary>
    public partial class FF14Watcher
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static FF14Watcher instance;

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        private Thread watchThread;
        private bool watchThreadRunning;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static FF14Watcher Default
        {
            get
            {
                FF14Watcher.Initialize();
                return instance;
            }
        }

        /// <summary>
        /// スピークdelegate
        /// </summary>
        public Speak SpeakDelegate { get; set; }

        /// <summary>
        /// 後片付けをする
        /// </summary>
        public static void Deinitialize()
        {
            instance.watchThreadRunning = false;

            lock (lockObject)
            {
                if (instance != null)
                {
                    if (instance.watchThread != null)
                    {
                        instance.watchThread.Join(TimeSpan.FromSeconds(1));
                        if (instance.watchThread.IsAlive)
                        {
                            instance.watchThread.Abort();
                        }

                        instance.watchThread = null;
                    }

                    instance = null;
                }
            }
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public static void Initialize()
        {
            lock (lockObject)
            {
                if (instance != null)
                {
                    return;
                }

                instance = new FF14Watcher();
                instance.Start();
            }
        }

        /// <summary>
        /// スピーク
        /// </summary>
        /// <param name="textToSpeak">喋る文字列</param>
        public void Speak(
            string textToSpeak)
        {
            this.SpeakDelegate?.Invoke(textToSpeak);
        }

        private void Start()
        {
            lock (lockObject)
            {
                if (this.watchThread != null &&
                    this.watchThread.IsAlive)
                {
                    return;
                }

                this.watchThread = new Thread(() =>
                {
                    while (this.watchThreadRunning)
                    {
                        try
                        {
                            this.WatchCore();
                        }
                        catch (ThreadAbortException)
                        {
                            this.watchThreadRunning = false;
                            return;
                        }
                        catch (Exception ex)
                        {
                            ActGlobals.oFormActMain.WriteExceptionLog(
                                ex,
                                "ACT.TTSYukkuri FF14の監視スレッドで例外が発生しました");
                        }

                        Thread.Sleep(400);
                    }
                });

                this.watchThreadRunning = true;
                this.watchThread.Start();
            }
        }

        /// <summary>
        /// 監視の中核
        /// </summary>
        private void WatchCore()
        {
            // ACTが表示されていなければ何もしない
            if (!ActGlobals.oFormActMain.Visible)
            {
                return;
            }

            // FF14Processがなければ何もしない
            if (FF14PluginHelper.GetFFXIVProcess == null)
            {
                return;
            }

            // オプションが全部OFFならば何もしない
            if (!TTSYukkuriConfig.Default.OptionSettings.EnabledHPWatch &&
                !TTSYukkuriConfig.Default.OptionSettings.EnabledMPWatch &&
                !TTSYukkuriConfig.Default.OptionSettings.EnabledTPWatch)
            {
                return;
            }

            // パーティメンバの監視を行う
            this.WatchParty();
        }
    }
}
