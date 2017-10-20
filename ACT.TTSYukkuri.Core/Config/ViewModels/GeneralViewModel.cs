using System.Diagnostics;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class GeneralViewModel : BindableBase
    {
        public TTSYukkuriConfig Config => TTSYukkuriConfig.Default;

        public ComboBoxItem[] TTSTypes => TTSType.ToComboBox;

        private ICommand playTTSCommand;

        public ICommand PlayTTSCommand =>
            this.playTTSCommand ?? (this.playTTSCommand = new DelegateCommand<string>((tts) =>
            {
                if (string.IsNullOrWhiteSpace(tts))
                {
                    return;
                }

                PluginCore.Instance.Speak(tts);
            }));

        private ICommand openCachFolderCommand;

        public ICommand OpenCachFolderCommand =>
            this.openCachFolderCommand ?? (this.openCachFolderCommand = new DelegateCommand(() =>
            {
                Process.Start(SpeechControllerExtentions.CacheDirectory);
            }));
    }
}
