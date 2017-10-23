using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ACT.TTSYukkuri.Config;
using Advanced_Combat_Tracker;

namespace ACT.TTSYukkuri.Boyomichan
{
    /// <summary>
    /// 棒読みちゃんスピーチコントローラ
    /// </summary>
    public class BoyomichanSpeechController :
        ISpeechController
    {
        /// <summary>
        /// 棒読みちゃんサーバ
        /// </summary>
        public const string BoyomichanServer = "127.0.0.1";

        /// <summary>
        /// 棒読みちゃんサーバのポート
        /// </summary>
        public const int BoyomichanServicePort = 50001;

        /// <summary>
        /// 棒読みちゃんへのCommand 0:メッセージ読上げ
        /// </summary>
        private const short BoyomiCommand = 0x0001;

        /// <summary>
        /// 棒読みちゃんの早さ -1:棒読みちゃんの画面上の設定に従う
        /// </summary>
        private const short BoyomiSpeed = -1;

        /// <summary>
        /// 棒読みちゃんへのテキストのエンコード 0:UTF-8
        /// </summary>
        private const byte BoyomiTextEncoding = 0;

        /// <summary>
        /// 棒読みちゃんの音程 -1:棒読みちゃんの画面上の設定に従う
        /// </summary>
        private const short BoyomiTone = -1;

        /// <summary>
        /// 棒読みちゃんの声質 0:棒読みちゃんの画面上の設定に従う
        /// </summary>
        private const short BoyomiVoice = 0;

        /// <summary>
        /// 棒読みちゃんの音量 -1:棒読みちゃんの画面上の設定に従う
        /// </summary>
        private const short BoyomiVolume = -1;

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize() { }

        public void Free()
        {
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public void Speak(
            string text)
        {
            // 初期化する
            this.Initialize();

            try
            {
                var server = TTSYukkuriConfig.Default.BoyomiServer;
                var port = TTSYukkuriConfig.Default.BoyomiPort;

                using (var tcp = new TcpClient(server, port))
                using (var ns = tcp.GetStream())
                using (var bw = new BinaryWriter(ns))
                {
                    var messageAsBytes = Encoding.UTF8.GetBytes(text);

                    bw.Write(BoyomiCommand);
                    bw.Write(BoyomiSpeed);
                    bw.Write(BoyomiTone);
                    bw.Write(BoyomiVolume);
                    bw.Write(BoyomiVoice);
                    bw.Write(BoyomiTextEncoding);
                    bw.Write(messageAsBytes.Length);
                    bw.Write(messageAsBytes);

                    bw.Flush();
                }
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "ACT.TTSYukkuri 棒読みちゃんの読上げで例外が発生しました");
            }
        }
    }
}
