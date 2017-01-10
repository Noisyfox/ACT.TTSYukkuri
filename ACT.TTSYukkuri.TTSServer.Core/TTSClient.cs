namespace ACT.TTSYukkuri.TTSServer.Core
{
    using System.ServiceModel;

    public class TTSClient
    {
        #region Singleton

        private static TTSClient instance;

        public static TTSClient Instance => instance ?? (instance = new TTSClient());

        #endregion

        public ITTSServerContract Channel { get; private set; }

        public void Open()
        {
            this.Channel = new ChannelFactory<ITTSServerContract>(
                new NetNamedPipeBinding(),
                new EndpointAddress(TTSServerConstants.ServiceEndPointUri)
                ).CreateChannel();
        }
    }
}
