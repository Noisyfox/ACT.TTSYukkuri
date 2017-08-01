﻿namespace ACT.TTSYukkuri.Sasara
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.TTSServer.Core;
    using ACT.TTSYukkuri.TTSServer.Core.Models;

    /// <summary>
    /// さとうささらスピーチコントローラ
    /// </summary>
    public class SasaraSpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// TTSの設定Panel
        /// </summary>
        public override UserControl TTSSettingsPanel => SasaraSettingsPanel.Default;

        /// <summary>
        /// 初期化する
        /// </summary>
        public override void Initialize()
        {
            TTSClient.Instance.Channel.StartSasara();
            TTSYukkuriConfig.Default.SetSasara();
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            // 今回の再生データをMD5化したものからwaveファイルの名称を作る
            var wave = ("Sasara" + TTSYukkuriConfig.Default.SasaraSettings.GetMD5() + text).GetMD5() + ".wav";
            wave = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"anoyetta\ACT\tts cache\" + wave);

            if (!Directory.Exists(Path.GetDirectoryName(wave)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(wave));
            }

            lock (this)
            {
                if (!File.Exists(wave))
                {
                    // 音声waveファイルを生成する
                    TTSClient.Instance.Channel.Speak(new Speak()
                    {
                        TTSEngine = TTSEngine.CeVIO,
                        TextToSpeak = text,
                        WaveFileName = wave,
                        Settings = TTSYukkuriConfig.Default.GetSasaraSettings(),
                    });
                }
            }

            // サブデバイスで再生する
            if (TTSYukkuriConfig.Default.EnabledSubDevice)
            {
                SoundPlayerWrapper.Play(
                    TTSYukkuriConfig.Default.SubDeviceID,
                    wave,
                    (int)TTSYukkuriConfig.Default.SasaraSettings.Onryo);
            }

            // メインデバイスで再生する
            SoundPlayerWrapper.Play(
                TTSYukkuriConfig.Default.MainDeviceID,
                wave,
                (int)TTSYukkuriConfig.Default.SasaraSettings.Onryo);
        }
    }
}
