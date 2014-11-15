namespace ACT.TTSYukkuri
{
    using System;

    using Advanced_Combat_Tracker;

    /// <summary>
    /// TTSゆっくりプラグイン
    /// </summary>
    public partial class TTSYukkuriPlugin : IActPluginV1
    {
        /// <summary>
        /// フラグインフォーム
        /// </summary>
        private class FormActMain_newTTS : FormActMain
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="args">プラグインへの引数</param>
            public FormActMain_newTTS(string[] args)
                : base(args)
            {
            }

            /// <summary>
            /// TTSを提供する
            /// </summary>
            /// <param name="sMessage">読上げるメッセージ</param>
            public void newTTS(string sMessage)
            {
                const string waitCommand = "/wait";

                try
                {
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
                catch (Exception ex)
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(
                        ex,
                        "TTSYukkuri newTTSで例外が発生しました。");
                }
            }
        }
    }
}
