namespace ACT.TTSYukkuri.TTSServer.Core.Models
{
    using System;

    [Serializable]
    public class SasaraSettings
    {
        public uint Volume { get; set; }
        public uint Speed { get; set; }
        public uint Tone { get; set; }
        public uint Alpha { get; set; }
        public uint ToneScale { get; set; }
        public string Cast { get; set; }
        public string[] AvailableCasts { get; set; }
        public SasaraTalkerComponent[] Components { get; set; }
    }
}
