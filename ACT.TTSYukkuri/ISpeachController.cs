namespace ACT.TTSYukkuri
{
    /// <summary>
    /// スピーチコントローラの汎用インターフェース
    /// </summary>
    public interface ISpeechController
    {
        /// <summary>
        /// TTSに話してもらう
        /// </summary>
        /// <param name="text">読上げるテキスト</param>
        void Speak(string text);

        /// <summary>
        /// TTSにディレイ後に話してもらう
        /// </summary>
        /// <param name="text">読上げるテキスト</param>
        /// <param name="delay">ディレイ(秒)</param>
        void SpeakWithDelay(string text, int delay);
    }
}
