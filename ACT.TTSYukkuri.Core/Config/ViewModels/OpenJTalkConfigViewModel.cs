using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class OpenJTalkConfigViewModel : BindableBase
    {
        public OpenJTalkConfig Config => Settings.Default.OpenJTalkSettings;

        public OpenJTalkVoice[] Voices => Settings.Default.OpenJTalkSettings.EnumlateVoice();
    }
}
