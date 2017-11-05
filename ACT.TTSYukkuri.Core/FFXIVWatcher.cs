using System;
using System.ComponentModel;
using System.Threading;
using ACT.TTSYukkuri.Config;
using Advanced_Combat_Tracker;
using FFXIV.Framework.Common;
using NLog;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// スピークdelegate
    /// </summary>
    /// <param name="textToSpeak"></param>
    public delegate void Speak(string textToSpeak);

    /// <summary>
    /// FF14を監視する
    /// </summary>
    public partial class FFXIVWatcher
    {
        #region Logger

        private Logger Logger => AppLog.DefaultLogger;

        #endregion Logger

        private const int WatcherInterval = 400;
        private const int WatcherLongInterval = 5000;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static FFXIVWatcher instance;

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        private BackgroundWorker watchWorker;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static FFXIVWatcher Default
        {
            get
            {
                FFXIVWatcher.Initialize();
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
            lock (lockObject)
            {
                if (instance != null)
                {
                    instance.watchWorker?.CancelAsync();
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

                instance = new FFXIVWatcher();
                instance.Start();
            }
        }

        /// <summary>
        /// スピーク
        /// </summary>
        /// <param name="textToSpeak">喋る文字列</param>
        public void Speak(
            string textToSpeak) =>
            this.SpeakDelegate?.Invoke(textToSpeak);

        private void Start()
        {
            lock (lockObject)
            {
                if (this.watchWorker != null)
                {
                    return;
                }

                this.watchWorker = new BackgroundWorker();
                this.watchWorker.WorkerSupportsCancellation = true;
                this.watchWorker.DoWork += (s, e) =>
                {
                    while (true)
                    {
                        try
                        {
                            if (this.watchWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }

                            this.WatchCore();
                        }
                        catch (Exception ex)
                        {
                            this.Logger.Error(ex, "FFXIVの監視スレッドで例外が発声しました。");
                            Thread.Sleep(WatcherLongInterval);
                        }

                        Thread.Sleep(WatcherInterval);
                    }
                };

                this.watchWorker.RunWorkerAsync();
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
                Thread.Sleep(WatcherLongInterval);
                return;
            }

            // FF14Processがなければ何もしない
            if (FFXIVPluginHelper.GetFFXIVProcess == null)
            {
                Thread.Sleep(WatcherLongInterval);
                return;
            }

            // オプションが全部OFFならば何もしない
            if (!Settings.Default.StatusAlertSettings.EnabledHPAlert &&
                !Settings.Default.StatusAlertSettings.EnabledMPAlert &&
                !Settings.Default.StatusAlertSettings.EnabledTPAlert)
            {
                Thread.Sleep(WatcherLongInterval);
                return;
            }

            // パーティメンバの監視を行う
            this.WatchParty();
        }
    }
}
