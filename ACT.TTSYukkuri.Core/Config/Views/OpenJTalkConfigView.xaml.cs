using System.Windows.Controls;
using ACT.TTSYukkuri.Config.ViewModels;
using ACT.TTSYukkuri.resources;

namespace ACT.TTSYukkuri.Config.Views
{
    /// <summary>
    /// OpenJTalkConfigView.xaml の相互作用ロジック
    /// </summary>
    public partial class OpenJTalkConfigView : UserControl, ILocalizable
    {
        public OpenJTalkConfigView()
        {
            this.InitializeComponent();
            this.DataContext = new OpenJTalkConfigViewModel();

            this.SetLocale(TTSYukkuriConfig.Default.UILocale);
        }

        public OpenJTalkConfigViewModel ViewModel => this.DataContext as OpenJTalkConfigViewModel;

        public void SetLocale(Locales locale) => this.ReloadLocaleDictionary(locale);
    }
}
