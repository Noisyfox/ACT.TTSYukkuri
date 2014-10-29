namespace ACT.TTSYukkuri
{
    using Advanced_Combat_Tracker;

    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class TTSYukkuriPlugin : IActPluginV1
    {
        private class FormActMain_newTTS : FormActMain
        {
            public FormActMain_newTTS(string[] args)
                : base(args)
            {
            }

            public void newTTS(string sMessage)
            {
                SpeechController.Default.Speak(sMessage);
            }
        }
    }
}
