using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ACT.TTSYukkuri.Config;
using ACT.TTSYukkuri.Discord.Models;
using ACT.TTSYukkuri.Discord.Views;
using Prism.Commands;

namespace ACT.TTSYukkuri.Discord.ViewModels
{
    public class DiscordViewModel
    {
        public DiscordView View { get; set; }

        public DiscordSettings Config => TTSYukkuriConfig.Default.DiscordSettings;
        public DiscordClientModel Model => DiscordClientModel.Instance;

        private ICommand connectCommand;
        private ICommand joinCommand;
        private ICommand leaveCommand;

        public ICommand ConnectCommand =>
            this.connectCommand ?? (this.connectCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.View.ConnectLink.IsEnabled = false;
                    this.Model.Connect();
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
                finally
                {
                    this.View.ConnectLink.IsEnabled = true;
                }
            }));

        public ICommand JoinCommand =>
            this.joinCommand ?? (this.joinCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.View.JoinLink.IsEnabled = false;
                    this.Model.Join();
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
                finally
                {
                    this.View.JoinLink.IsEnabled = true;
                }
            }));

        public ICommand LeaveCommand =>
            this.leaveCommand ?? (this.leaveCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.View.LeaveLink.IsEnabled = false;
                    this.Model.Leave();
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
                finally
                {
                    this.View.LeaveLink.IsEnabled = true;
                }
            }));
    }
}
