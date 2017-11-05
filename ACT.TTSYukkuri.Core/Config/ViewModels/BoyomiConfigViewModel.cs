using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class BoyomiConfigViewModel : BindableBase
    {
        public TTSYukkuriConfig Config => TTSYukkuriConfig.Default;
    }
}
