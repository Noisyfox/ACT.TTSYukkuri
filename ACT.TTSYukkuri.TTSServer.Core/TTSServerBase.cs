namespace ACT.TTSYukkuri.TTSServer.Core
{
    using System;
    using System.ServiceModel;

    using ACT.TTSYukkuri.TTSServer.Core.Models;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public abstract class TTSServerBase<T> :
        ITTSServerContract
        where T : TTSServerBase<T>, new()
    {
        #region Singleton

        private static T instance = new T();

        public static T Instance => instance;

        #endregion Singleton

        protected TTSServerBase()
        {
        }

        public ServiceHost Host { get; private set; }

        public void CloseServer()
        {
            if (this.IsReady())
            {
                this.Host.Close();
            }
        }

        public abstract void End();

        public abstract void EndSasara();

        public abstract SasaraSettings GetSasaraSettings();

        public bool IsReady()
        {
            if (this.Host != null &&
                this.Host.State == CommunicationState.Opened)
            {
                return true;
            }

            return false;
        }

        public void OpenServer()
        {
            this.Host = new ServiceHost(
                new T(),
                new Uri(TTSServerConstants.HostUri));

            this.Host.AddServiceEndpoint(
                typeof(ITTSServerContract),
                new NetNamedPipeBinding(),
                TTSServerConstants.ServiceEndPoint);

            this.Host.Open();
        }

        public abstract void SetSasaraSettings(SasaraSettings settings);

        public abstract void Speak(Speak speakModel);

        public abstract void StartSasara();
    }
}
