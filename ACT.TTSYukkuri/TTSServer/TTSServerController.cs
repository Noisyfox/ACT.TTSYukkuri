using FFXIV.Framework.TTS.Common;

namespace ACT.TTSYukkuri.TTSServer
{
    public static class TTSServerController
    {
        public static void End()
        {
            TTSClient.Instance.Close();
#if DEBUG
            TTSClient.Instance.ShutdownTTSServer();
#endif
        }

        public static void Start()
        {
            TTSClient.Instance.StartTTSServer();
            TTSClient.Instance.Open();
        }
    }
}
