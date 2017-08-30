using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ACT.TTSYukkuri.Config
{
    [Serializable]
    public class DiscordSettings :
        INotifyPropertyChanged
    {
        private string token = string.Empty;
        private string defaultServer = string.Empty;
        private ulong defaultChannelID = 0;
        private bool autoJoin = false;

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

        #region INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(
            [CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            this.PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion INotifyPropertyChanged
    }
}
