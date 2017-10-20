using System.Windows.Controls;
using ACT.TTSYukkuri.Config.ViewModels;
using ACT.TTSYukkuri.resources;

namespace ACT.TTSYukkuri.Config.Views
{
    /// <summary>
    /// StatusAlertConfigView.xaml の相互作用ロジック
    /// </summary>
    public partial class StatusAlertConfigView : UserControl, ILocalizable
    {
        public StatusAlertConfigView()
        {
            this.InitializeComponent();
            this.DataContext = new StatusAlertConfigViewModel();

            this.SetLocale(TTSYukkuriConfig.Default.UILocale);
        }

        public StatusAlertConfigViewModel ViewModel => this.DataContext as StatusAlertConfigViewModel;

        public void SetLocale(Locales locale) => this.ReloadLocaleDictionary(locale);
    }
}
