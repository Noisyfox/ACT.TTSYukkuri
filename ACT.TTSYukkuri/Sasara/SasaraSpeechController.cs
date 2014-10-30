namespace ACT.TTSYukkuri.Sasara
{
    using CeVIO.Talk.RemoteService;

    /// <summary>
    /// さとうささらスピーチコントローラ
    /// </summary>
    public class SasaraSpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// ささらリモートインターフェースクラス
        /// </summary>
        private Talker talker;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SasaraSpeechController()
        {
            this.talker = new Talker();

            // CeVIO Creative Studio を起動する
            if (!ServiceControl.IsHostStarted)
            {
                ServiceControl.StartHost(false);
            }
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            // CeVIO Creative Studio を起動する
            if (!ServiceControl.IsHostStarted)
            {
                ServiceControl.StartHost(false);
            }

            var casts = Talker.AvailableCasts;

            if (casts.Length > 0)
            {
                talker.Cast = casts[0];

                // テキストを読上げる
                talker.Speak(text);
            }
        }
    }
}
