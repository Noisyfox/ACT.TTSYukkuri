namespace ACT.TTSYukkuri.OpenJTalk
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using NAudio.Wave;

    public class OpenJTalkSpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// TTSの設定Panel
        /// </summary>
        public override UserControl TTSSettingsPanel
        {
            get
            {
                return OpenJTalkSettingsPanel.Default;
            }
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public override void Initialize()
        {
            // NO-OP
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            lock (lockObject)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                // 現在の条件からwaveファイル名を生成する
                var wave = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"anoyetta\ACT\" + ("OpenJTalk" + TTSYukkuriConfig.Default.OpenJTalkSettings.ToString() + text).GetMD5() + ".wav");

                if (!File.Exists(wave))
                {
                    this.CreateWave(
                        text,
                        wave);
                }

                // サブデバイスを再生する
                // サブデバイスは専らVoiceChat用であるため先に鳴動させる
                if (TTSYukkuriConfig.Default.EnabledSubDevice)
                {
                    SoundPlayerWrapper.Play(
                        TTSYukkuriConfig.Default.SubDeviceNo,
                        wave,
                        TTSYukkuriConfig.Default.OpenJTalkSettings.Volume);
                }

                // メインデバイスを再生する
                SoundPlayerWrapper.Play(
                    TTSYukkuriConfig.Default.MainDeviceNo,
                    wave,
                    TTSYukkuriConfig.Default.OpenJTalkSettings.Volume);
            }
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
            // パス関係を生成する
            var openJTalkDir = TTSYukkuriConfig.Default.OpenJTalkSettings.OpenJTalkDirectory;
            if (string.IsNullOrWhiteSpace(openJTalkDir))
            {
                openJTalkDir = "OpenJTalk";
            }

            var openJTalk = Path.Combine(openJTalkDir, @"open_jtalk.exe");
            var dic = Path.Combine(openJTalkDir, @"dic");
            var voice = Path.Combine(openJTalkDir, @"voice\" + TTSYukkuriConfig.Default.OpenJTalkSettings.Voice);
            var waveTemp = Path.GetTempFileName();
            if (File.Exists(waveTemp))
            {
                File.Delete(waveTemp);
            }

            var tone = (float)TTSYukkuriConfig.Default.OpenJTalkSettings.Tone / 100f;
            var speed = (float)TTSYukkuriConfig.Default.OpenJTalkSettings.Speed / 100f;

            var textFile = Path.GetTempFileName();
            File.WriteAllText(textFile, textToSpeak, Encoding.GetEncoding("Shift_JIS"));

            var args = new string[]
            {
                "-x " + "\"" + dic + "\"",
                "-m " + "\"" + voice + "\"",
                "-ow " + "\"" + waveTemp + "\"",
                "-b " + tone.ToString("N1"),
                "-r " + speed.ToString("N1"),
                textFile
            };

            var pi = new ProcessStartInfo()
            {
                FileName = openJTalk,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = string.Join(" ", args),
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            Debug.WriteLine(pi.FileName + " " + pi.Arguments);

            using (var p = Process.Start(pi))
            {
                var stderr = p.StandardError.ReadToEnd();
                var stdout = p.StandardOutput.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(stderr))
                {
                    Debug.WriteLine(stderr);
                }

                if (!string.IsNullOrWhiteSpace(stdout))
                {
                    Debug.WriteLine(stdout);
                }

                p.WaitForExit();
            }

            if (File.Exists(textFile))
            {
                File.Delete(textFile);
            }

            using (var reader = new WaveFileReader(waveTemp))
            {
                var prov = new VolumeWaveProvider16(reader);
                prov.Volume = (float)TTSYukkuriConfig.Default.OpenJTalkSettings.Gain / 100f;

                WaveFileWriter.CreateWaveFile(
                    wave,
                    prov);
            }

            if (File.Exists(waveTemp))
            {
                File.Delete(waveTemp);
            }
        }
    }
}
