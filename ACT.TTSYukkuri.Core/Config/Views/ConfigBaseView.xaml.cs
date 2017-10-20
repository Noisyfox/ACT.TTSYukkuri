using System.Windows.Controls;
using ACT.TTSYukkuri.resources;

namespace ACT.TTSYukkuri.Config.Views
{
    /// <summary>
    /// ConfigBaseView.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigBaseView : UserControl, ILocalizable
    {
        public ConfigBaseView()
        {
            this.InitializeComponent();

            this.SetLocale(TTSYukkuriConfig.Default.UILocale);
        }

        public void SetLocale(Locales locale) => this.ReloadLocaleDictionary(locale);
    }
}
