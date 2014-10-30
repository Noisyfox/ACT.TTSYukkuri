namespace ACT.TTSYukkuri
{
    using ACT.TTSYukkuri.Properties;
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
                        nowTTSType != Settings.Default.TTSType)
                    {
                        switch (Settings.Default.TTSType)
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

                        nowTTSType = Settings.Default.TTSType;
                    }

                    return instance;
                }
            }
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
