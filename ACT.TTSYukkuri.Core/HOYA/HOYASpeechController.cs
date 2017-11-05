using System.IO;
using ACT.TTSYukkuri.Config;
using VoiceTextWebAPI.Client;

namespace ACT.TTSYukkuri.HOYA
{
    public class HOYASpeechController :
        ISpeechController
    {
        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
        }

        public void Free()
        {
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public void Speak(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            // 現在の条件をハッシュ化してWAVEファイル名を作る
            var wave = this.GetCacheFileName(
                TTSYukkuriConfig.Default.TTS,
                text,
                TTSYukkuriConfig.Default.HOYASettings.ToString());

            lock (this)
            {
                if (!File.Exists(wave))
                {
                    if (string.IsNullOrWhiteSpace(
                        TTSYukkuriConfig.Default.HOYASettings.APIKey))
                    {
                        return;
                    }

                    this.CreateWave(
                        text,
                        wave);
                }
            }

            // 再生する
            SoundPlayerWrapper.Play(wave);
        }

        /// <summary>
        /// WAVEファイルを生成する
        /// </summary>
        /// <param name="textToSpeak">
        /// Text to Speak</param>
        /// <param name="wave">
        /// WAVEファイルのパス</param>
        private void CreateWave(
            string textToSpeak,
            string wave)
        {
            var client = new VoiceTextClient()
            {
                APIKey = TTSYukkuriConfig.Default.HOYASettings.APIKey,
                Speaker = TTSYukkuriConfig.Default.HOYASettings.Speaker,
                Emotion = TTSYukkuriConfig.Default.HOYASettings.Emotion,
                EmotionLevel = TTSYukkuriConfig.Default.HOYASettings.EmotionLevel,
                Volume = TTSYukkuriConfig.Default.HOYASettings.Volume,
                Speed = TTSYukkuriConfig.Default.HOYASettings.Speed,
                Pitch = TTSYukkuriConfig.Default.HOYASettings.Pitch,
            };

            var waveData = client.GetVoice(textToSpeak);

            File.WriteAllBytes(wave, waveData);
        }
    }
}
