namespace ACT.TTSYukkuri.TTSServer.Core
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class TTSClient
    {
        #region Singleton

        private static TTSClient instance;

        public static TTSClient Instance => instance ?? (instance = new TTSClient());

        #endregion

        private ITTSServerContract channel;

        public ITTSServerContract Channel
        {
            get
            {
                if (this.channel != null)
                {
                    var proxy = this.channel as IChannel;
                    if (proxy != null)
                    {
                        if (proxy.State == CommunicationState.Faulted ||
                            proxy.State == CommunicationState.Closed)
                        {
                            this.Open();
                        }
                    }
                }

                return this.channel;
            }
        }

        public void Open()
        {
            this.channel = new ChannelFactory<ITTSServerContract>(
                new NetNamedPipeBinding(),
                new EndpointAddress(TTSServerConstants.ServiceEndPointUri)
                ).CreateChannel();
        }
    }
}
