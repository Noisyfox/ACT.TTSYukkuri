namespace VoiceTextWebAPI.Client.Internal
{
    public class VoiceTextErrorResponse
    {
        public class VoiceTextError
        {
            public string Message { get; set; }
        } 

        public VoiceTextError Error { get; set; }
    }
}
