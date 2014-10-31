namespace ACT.TTSYukkuri
{
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.Sasara;
    using ACT.TTSYukkuri.Yukkuri;

    /// <summary>
    /// スピーチコントローラ
    /// </summary>
    public class SpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// Lockオブジェクト
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// シングルトンinstance
        /// </summary>
        private static ISpeechController instance;

        /// <summary>
        /// 現在のTTSタイプ
        /// </summary>
        private static string nowTTSType;

        /// <summary>
        /// シングルトンinstanceを返す
        /// </summary>
        public static ISpeechController Default
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null ||
                        nowTTSType != TTSYukkuriConfig.Default.TTS)
                    {
                        switch (TTSYukkuriConfig.Default.TTS)
                        {
                            case TTSType.Yukkuri:
                                instance = new YukkuriSpeechController();
                                break;

                            case TTSType.SasaraSato:
                                instance = new SasaraSpeechController();
                                break;

                            default:
                                instance = new YukkuriSpeechController();
                                break;
                        }

                        nowTTSType = TTSYukkuriConfig.Default.TTS;
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// TTSの設定Panel
        /// </summary>
        public override UserControl TTSSettingsPanel
        {
            get
            {
                return SpeechController.Default.TTSSettingsPanel;
            }
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public override void Initialize()
        {
            SpeechController.Default.Initialize();
        }

        /// <summary>
        /// TTSに話してもらう
        /// </summary>
        /// <param name="text">読上げるテキスト</param>
        public override void Speak(string text)
        {
            instance.Speak(text);
        }
    }
}
