using System.Collections.Generic;
using System.Speech.Synthesis;
using ACT.TTSYukkuri.SAPI5;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class SAPI5ConfigViewModel : BindableBase
    {
        public SAPI5Configs Config => Settings.Default.SAPI5Settings;

        public IReadOnlyList<InstalledVoice> Voices => SAPI5SpeechController.EnumerateSynthesizers();
    }
}
