using System.Collections.Generic;
using ACT.TTSYukkuri.Yukkuri;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class YukkuriConfigViewModel : BindableBase
    {
        public YukkuriConfig Config => TTSYukkuriConfig.Default.YukkuriSettings;

        public IReadOnlyList<AQPreset> Presets => AQVoicePresets.Presets;
    }
}
