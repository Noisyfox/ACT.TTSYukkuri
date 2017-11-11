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
        private ulong defaultTextChannelID = 0;
        private ulong defaultVoiceChannelID = 0;
        private bool autoJoin = false;
        private double speakInterval = 0.6d;

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

        public ulong DefaultTextChannelID
        {
            get => this.defaultTextChannelID;
            set => this.SetProperty(ref this.defaultTextChannelID, value);
        }

        public ulong DefaultVoiceChannelID
        {
            get => this.defaultVoiceChannelID;
            set => this.SetProperty(ref this.defaultVoiceChannelID, value);
        }

        public bool AutoJoin
        {
            get => this.autoJoin;
            set => this.SetProperty(ref this.autoJoin, value);
        }

        public double SpeakInterval
        {
            get => this.speakInterval;
            set => this.SetProperty(ref this.speakInterval, value);
        }
    }
}
