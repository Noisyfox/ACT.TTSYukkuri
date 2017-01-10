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

        private static T instance;

        public static T Instance => instance ?? (instance = new T());

        #endregion

        protected TTSServerBase()
        {
        }

        public ServiceHost Host { get; private set; }

        public bool IsReady()
        {
            if (this.Host != null &&
                this.Host.State == CommunicationState.Opened)
            {
                return true;
            }

            return false;
        }

        public abstract void Speak(Speak speakModel);

        public abstract SasaraSettings GetSasaraSettings();

        public abstract void SetSasaraSettings(SasaraSettings settings);

        public abstract void StartSasara();

        public abstract void EndSasara();

        public abstract void End();

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

        public void CloseServer()
        {
            if (this.IsReady())
            {
                this.Host.Close();
            }
        }
    }
}
