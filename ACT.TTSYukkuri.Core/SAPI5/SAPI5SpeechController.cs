using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using ACT.TTSYukkuri.Config;

namespace ACT.TTSYukkuri.SAPI5
{
    public enum Pitches
    {
        Default = 0,
        XLow = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        XHigh = 5,
    }

    public static class PitchesExtensions
    {
        public static string ToXML(
            this Pitches p)
            => new[]
            {
                "default",
                "x-low",
                "low",
                "medium",
                "high",
                "x-high",
            }[(int)p];
    }

    public class SAPI5SpeechController :
        ISpeechController
    {
        private static IReadOnlyList<InstalledVoice> synthesizers;

        public static IReadOnlyList<InstalledVoice> Synthesizers =>
            synthesizers ?? (synthesizers = (new SpeechSynthesizer()).GetInstalledVoices());

        private SAPI5Configs Config => Settings.Default.SAPI5Settings;

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 開放する
        /// </summary>
        public void Free()
        {
        }

        private readonly SpeechAudioFormatInfo WAVEFormat = new SpeechAudioFormatInfo(
            32000,
            AudioBitsPerSample.Sixteen,
            AudioChannel.Mono);

        private InstalledVoice GetSynthesizer(
            string id)
            => Synthesizers.FirstOrDefault(x => x.VoiceInfo.Id == id);

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public void Speak(
            string text,
            PlayDevices playDevice = PlayDevices.Both)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

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
                    using (var synth = new SpeechSynthesizer())
                    {
                        // VOICEを設定する
                        if (synth.Voice.Id != this.Config.VoiceID)
                        {
                            var voice = this.GetSynthesizer(this.Config.VoiceID);
                            if (voice == null)
                            {
                                return;
                            }

                            synth.SelectVoice(voice.VoiceInfo.Name);
                        }

                        synth.Rate = this.Config.Rate;
                        synth.Volume = this.Config.Volume;

                        // Promptを生成する
                        var pb = new PromptBuilder();
                        pb.AppendSsmlMarkup(
                            $"<prosody pitch=\"{this.Config.Pitch.ToXML()}\">{text}</prosody>");

                        synth.SetOutputToWaveStream(fs);
                        synth.Speak(pb);
                    }
                }
            }

            // 再生する
            SoundPlayerWrapper.Play(wave, playDevice);
        }
    }
}
