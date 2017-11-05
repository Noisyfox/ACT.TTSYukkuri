using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class OpenJTalkConfigViewModel : BindableBase
    {
        public OpenJTalkConfig Config => TTSYukkuriConfig.Default.OpenJTalkSettings;

        public OpenJTalkVoice[] Voices => TTSYukkuriConfig.Default.OpenJTalkSettings.EnumlateVoice();
    }
}
