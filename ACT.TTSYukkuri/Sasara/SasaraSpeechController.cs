namespace ACT.TTSYukkuri.Sasara
{
    using System.Windows.Forms;

    using CeVIO.Talk.RemoteService;

    /// <summary>
    /// さとうささらスピーチコントローラ
    /// </summary>
    public class SasaraSpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// ささらリモートインターフェースクラス
        /// </summary>
        public static Talker Talker
        {
            get;
            private set;
        }


        /// <summary>
        /// TTSの設定Panel
        /// </summary>
        public override UserControl TTSSettingsPanel
        {
            get
            {
                return SasaraSettingsPanel.Default;
            }
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public override void Initialize()
        {
            lock (lockObject)
            {
                // CeVIO Creative Studio を起動する
                if (!ServiceControl.IsHostStarted)
                {
                    ServiceControl.StartHost(false);
                }

                if (Talker == null)
                {
                    Talker = new Talker();
                }
            }
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            // 初期化する
            this.Initialize();

            if (!string.IsNullOrWhiteSpace(Talker.Cast))
            {
                // テキストを読上げる
                Talker.Speak(text);
            }
        }
    }
}
