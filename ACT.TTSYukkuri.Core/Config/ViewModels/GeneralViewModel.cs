using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config.ViewModels
{
    public class GeneralViewModel : BindableBase
    {
        public Settings Config => Settings.Default;

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

        private ICommand changePlayMethodCommand;

        public ICommand ChangePlayMethodCommand =>
            this.changePlayMethodCommand ?? (this.changePlayMethodCommand = new DelegateCommand(() =>
            {
                var devices = this.Config.PlayDevices;
                if (!devices.Any(x => x.ID == this.Config.MainDeviceID))
                {
                    this.Config.MainDeviceID = devices.FirstOrDefault()?.ID;
                }

                if (!devices.Any(x => x.ID == this.Config.SubDeviceID))
                {
                    this.Config.SubDeviceID = devices.FirstOrDefault()?.ID;
                }
            }));
    }
}
