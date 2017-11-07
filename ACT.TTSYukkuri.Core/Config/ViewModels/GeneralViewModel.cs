using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        private ICommand openCacheFolderCommand;

        public ICommand OpenCacheFolderCommand =>
            this.openCacheFolderCommand ?? (this.openCacheFolderCommand = new DelegateCommand(() =>
            {
                if (Directory.Exists(SpeechControllerExtentions.CacheDirectory))
                {
                    Process.Start(SpeechControllerExtentions.CacheDirectory);
                }
            }));

        private ICommand changePlayMethodCommand;

        public ICommand clearCacheCommand;
        public ICommand ClearCacheCommand =>
            this.clearCacheCommand ?? (this.clearCacheCommand = new DelegateCommand(async () =>
            {
                if (Directory.Exists(SpeechControllerExtentions.CacheDirectory))
                {
                    await Task.Run(() =>
                    {
                        foreach (var file in Directory.GetFiles(
                            SpeechControllerExtentions.CacheDirectory,
                            "*.wav",
                            SearchOption.TopDirectoryOnly))
                        {
                            File.Delete(file);
                        }
                    });

                    MessageBox.Show(
                        "Cached wave deleted.",
                        "ACT.UltraScouter",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }));

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
