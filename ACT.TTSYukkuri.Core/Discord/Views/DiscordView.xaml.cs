using System.Windows.Controls;
using ACT.TTSYukkuri.Discord.ViewModels;

namespace ACT.TTSYukkuri.Discord.Views
{
    /// <summary>
    /// DiscordView.xaml の相互作用ロジック
    /// </summary>
    public partial class DiscordView : UserControl
    {
        public DiscordView()
        {
            this.InitializeComponent();
            this.DataContext = new DiscordViewModel();
            this.ViewModel.View = this;
        }

        public DiscordViewModel ViewModel
        {
            get => this.DataContext as DiscordViewModel;
            set => this.DataContext = value;
        }
    }
}
