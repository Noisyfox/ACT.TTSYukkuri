using System;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config
{
    [Serializable]
    public class DiscordSettings :
        BindableBase
    {
        private string token = string.Empty;
        private string defaultServer = string.Empty;
        private ulong defaultChannelID = 0;
        private bool autoJoin = false;
        private double speakTimeout = 0.2d;

        public string Token
        {
            get => this.token;
            set => this.SetProperty(ref this.token, value);
        }

        public string DefaultServer
        {
            get => this.defaultServer;
            set => this.SetProperty(ref this.defaultServer, value);
        }

        public ulong DefaultChannelID
        {
            get => this.defaultChannelID;
            set => this.SetProperty(ref this.defaultChannelID, value);
        }

        public bool AutoJoin
        {
            get => this.autoJoin;
            set => this.SetProperty(ref this.autoJoin, value);
        }

        public double SpeakTimeout
        {
            get => this.speakTimeout;
            set => this.SetProperty(ref this.speakTimeout, value);
        }
    }
}
