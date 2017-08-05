namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.Threading;
    using ACT.TTSYukkuri.TTSServer.Core;
    using ACT.TTSYukkuri.TTSServer.Core.Models;

    public class TTSServer :
        TTSServerBase<TTSServer>
    {
        private bool isSasaraStarted;

        public override void End()
        {
            try
            {
                YukkuriController.Default.Free();

                if (this.isSasaraStarted)
                {
                    this.EndSasara();
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Program.WriteLineLog(
                    "Catched Exception on End", ex);
            }
        }

        public override void EndSasara()
        {
            SasaraController.Default.CloseSasara();
            isSasaraStarted = false;
        }

        public override SasaraSettings GetSasaraSettings()
        {
            var settings = SasaraController.Default.GetSasaraSettings();
            isSasaraStarted = true;
            return settings;
        }

        public override void SetSasaraSettings(SasaraSettings settings)
        {
            SasaraController.Default.SetSasaraSettings(settings);
            isSasaraStarted = true;
        }

        public override void Speak(Speak speakModel)
        {
            switch (speakModel.TTSEngine)
            {
                case TTSEngine.Yukkuri:
                    YukkuriController.Default.OutputWaveToFile(
                        speakModel.TextToSpeak,
                        (ushort)speakModel.SpeakSpeed,
                        speakModel.WaveFileName);
                    break;

                case TTSEngine.CeVIO:
                    SasaraController.Default.OutputWaveToFile(
                        speakModel.TextToSpeak,
                        speakModel.WaveFileName,
                        speakModel.Settings);

                    isSasaraStarted = true;
                    break;
            }
        }

        public override void StartSasara()
        {
            SasaraController.Default.StartSasara();
            isSasaraStarted = true;
        }
    }
}
