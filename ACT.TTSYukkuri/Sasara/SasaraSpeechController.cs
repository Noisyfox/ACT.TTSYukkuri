namespace ACT.TTSYukkuri.Sasara
{
    using System.IO;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using Advanced_Combat_Tracker;
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
        public static Talker Talker
        {
            get;
            private set;
        }

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
            if (ActGlobals.oFormActMain.InvokeRequired)
            {
                ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                {
                    // CeVIO Creative Studio を起動する
                    if (!ServiceControl.IsHostStarted)
                    {
                        ServiceControl.StartHost(false);
                    }

                    if (Talker == null)
                    {
                        Talker = new Talker();
                    }

                    // ささらを設定する
                    TTSYukkuriConfig.Default.SetSasara();
                });
            }
            else
            {
                // CeVIO Creative Studio を起動する
                if (!ServiceControl.IsHostStarted)
                {
                    ServiceControl.StartHost(false);
                }

                if (Talker == null)
                {
                    Talker = new Talker();
                }

                // ささらを設定する
                TTSYukkuriConfig.Default.SetSasara();
            }
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            if (ActGlobals.oFormActMain.InvokeRequired)
            {
                ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                {
                    // 初期化する
                    this.Initialize();

                    if (!string.IsNullOrWhiteSpace(Talker.Cast))
                    {
                        // サブデバイスで再生する
                        if (TTSYukkuriConfig.Default.EnabledSubDevice)
                        {
                            this.SpeakCore(
                                TTSYukkuriConfig.Default.SubDeviceNo,
                                text);
                        }

                        // メインデバイスで再生する
                        this.SpeakCore(
                            TTSYukkuriConfig.Default.MainDeviceNo,
                            text);
                    }
                });
            }
            else
            {
                // 初期化する
                this.Initialize();

                if (!string.IsNullOrWhiteSpace(Talker.Cast))
                {
                    // サブデバイスで再生する
                    if (TTSYukkuriConfig.Default.EnabledSubDevice)
                    {
                        this.SpeakCore(
                            TTSYukkuriConfig.Default.SubDeviceNo,
                            text);
                    }

                    // メインデバイスで再生する
                    this.SpeakCore(
                        TTSYukkuriConfig.Default.MainDeviceNo,
                        text);
                }
            }
        }

        /// <summary>
        /// デバイスを指定して読上げる
        /// </summary>
        /// <param name="deviceNo">デバイス番号</param>
        /// <param name="textToSpeak">読上げるテキスト</param>
        private void SpeakCore(
            int deviceNo,
            string textToSpeak)
        {
            // 一時ファイルのパスを取得する
            var file = Path.GetTempFileName();

            // 音声をwaveに出力させる
            Talker.OutputWaveToFile(
                textToSpeak,
                file);

            // サウンドプレイヤで再生する
            SoundPlayerWrapper.Play(
                deviceNo,
                file,
                (int)TTSYukkuriConfig.Default.SasaraSettings.Onryo);
        }
    }
}
