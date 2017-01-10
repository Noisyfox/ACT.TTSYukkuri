namespace ACT.TTSYukkuri.TTSServer
{
    using System;

    using ACT.TTSYukkuri.TTSServer.Core;
    using ACT.TTSYukkuri.TTSServer.Core.Models;

    public class TTSServer :
        TTSServerBase<TTSServer>
    {
        private static object lockObject = new object();

        private bool isSasaraStarted;

        public override void Speak(Speak speakModel)
        {
            lock (lockObject)
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
                            speakModel.WaveFileName);

                        isSasaraStarted = true;
                        break;
                }
            }
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

        public override void StartSasara()
        {
            lock (lockObject)
            {
                SasaraController.Default.StartSasara();
                isSasaraStarted = true;
            }
        }

        public override void EndSasara()
        {
            lock (lockObject)
            {
                SasaraController.Default.CloseSasara();
                isSasaraStarted = false;
            }
        }

        public override void End()
        {
            try
            {
                this.CloseServer();

                YukkuriController.Default.Free();

                if (this.isSasaraStarted)
                {
                    SasaraController.Default.CloseSasara();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Catched Exception on End");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
