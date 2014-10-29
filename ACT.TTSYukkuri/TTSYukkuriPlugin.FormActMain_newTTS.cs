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
                const string waitCommand = "/wait";

                // waitなし？
                if (!sMessage.StartsWith(waitCommand))
                {
                    SpeechController.Default.Speak(sMessage);
                }
                else
                {
                    var values = sMessage.Split(',');

                    // 分割できない？
                    if (values.Length < 2)
                    {
                        // 普通に読上げて終わる
                        SpeechController.Default.Speak(sMessage);
                        return;
                    }

                    var command = values[0].Trim();
                    var message = values[1].Trim();

                    // 秒数を取り出す
                    var delayAsText = command.Replace(waitCommand, string.Empty);
                    int delay = 0;
                    if (!int.TryParse(delayAsText, out delay))
                    {
                        // 普通に読上げて終わる
                        SpeechController.Default.Speak(sMessage);
                        return;
                    }

                    // ディレイをかけて読上げる
                    SpeechController.Default.SpeakWithDelay(
                        message,
                        delay);
                }
            }
        }
    }
}
