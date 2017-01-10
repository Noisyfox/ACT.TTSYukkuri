namespace ACT.TTSYukkuri.TTSServer.Core
{
    public static class TTSServerConstants
    {
        public const string HostUri = "net.pipe://localhost";

        public const string ServiceEndPoint = "ACT.TTSYukkuri.TTSServer";

        public static string ServiceEndPointUri => $"{HostUri}/{ServiceEndPoint}";
    }
}
