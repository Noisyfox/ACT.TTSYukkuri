namespace ACT.TTSYukkuri.TTSServer.Core.Models
{
    using System;

    [Serializable]
    public class Speak
    {
        /// <summary>TTSエンジン</summary>
        public TTSEngine TTSEngine { get; set; }

        /// <summary>音声化するためのテキスト</summary>
        public string TextToSpeak { get; set; }

        /// <summary>会話速度</summary>
        public int SpeakSpeed { get; set; }

        /// <summary>生成される音声ファイルのファイル名</summary>
        public string WaveFileName { get; set; }
    }
}
