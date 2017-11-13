using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using ACT.TTSYukkuri.Config;

namespace ACT.TTSYukkuri.SAPI5
{
    public class SAPI5SpeechController :
        ISpeechController
    {
        public static IReadOnlyList<InstalledVoice> EnumerateSynthesizers()
            => (new SpeechSynthesizer()).GetInstalledVoices();

        /// <summary>
        /// SpeechSynthesizer
        /// </summary>
        private SpeechSynthesizer synthesizer;

        private SAPI5Configs Config => Settings.Default.SAPI5Settings;

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
            this.synthesizer = new SpeechSynthesizer();
        }

        /// <summary>
        /// 開放する
        /// </summary>
        public void Free()
        {
            this.synthesizer.Dispose();
            this.synthesizer = null;
        }

        private readonly SpeechAudioFormatInfo WAVEFormat = new SpeechAudioFormatInfo(
            32000,
            AudioBitsPerSample.Sixteen,
            AudioChannel.Mono);

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

            // VOICEを設定する
            if (this.synthesizer.Voice.Id !=
                this.Config.VoiceID)
            {
                var voice = EnumerateSynthesizers().FirstOrDefault(x =>
                    x.VoiceInfo.Id == this.Config.VoiceID);

                if (voice == null)
                {
                    return;
                }

                this.synthesizer.SelectVoice(voice.VoiceInfo.Name);
            }

            this.synthesizer.Rate = this.Config.Rate;
            this.synthesizer.Volume = this.Config.Volume;

            // 現在の条件をハッシュ化してWAVEファイル名を作る
            var wave = this.GetCacheFileName(
                Settings.Default.TTS,
                text,
                this.Config.ToString());

            lock (this)
            {
                if (!File.Exists(wave))
                {
                    using (var fs = new FileStream(wave, FileMode.Create))
                    {
                        this.synthesizer.SetOutputToWaveStream(fs);
                        this.synthesizer.Speak(text);
                    }
                }
            }

            // 再生する
            SoundPlayerWrapper.Play(wave);
        }
    }
}
