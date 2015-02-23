namespace ACT.TTSYukkuri.Sasara
{
    using System.IO;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using ACT.TTSYukkuri.TTSServer;
    using ACT.TTSYukkuri.TTSServer.Core;

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
            TTSServerController.Message.StartSasara();
            TTSYukkuriConfig.Default.SetSasara();
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
                // 初期化する
                this.Initialize();

                // 音声waveファイルを生成する
                var e = new TTSMessage.SpeakEventArg()
                {
                    TTSType = TTSTEngineType.CeVIO,
                    TextToSpeack = text,
                    WaveFile = Path.GetTempFileName()
                };

                TTSServerController.Message.Speak(e);

                // サブデバイスで再生する
                if (TTSYukkuriConfig.Default.EnabledSubDevice)
                {
                    SoundPlayerWrapper.Play(
                        TTSYukkuriConfig.Default.SubDeviceNo,
                        e.WaveFile,
                        (int)TTSYukkuriConfig.Default.SasaraSettings.Onryo);
                }

                // メインデバイスで再生する
                SoundPlayerWrapper.Play(
                    TTSYukkuriConfig.Default.MainDeviceNo,
                    e.WaveFile,
                    (int)TTSYukkuriConfig.Default.SasaraSettings.Onryo);

                if (File.Exists(e.WaveFile))
                {
                    File.Delete(e.WaveFile);
                }
            }
        }
    }
}
