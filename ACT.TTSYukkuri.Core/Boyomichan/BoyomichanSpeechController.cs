using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ACT.TTSYukkuri.Config;

namespace ACT.TTSYukkuri.Boyomichan
{
    /// <summary>
    /// 棒読みちゃんスピーチコントローラ
    /// </summary>
    public class BoyomichanSpeechController :
        ISpeechController
    {
        #region Constants

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

        #endregion Constants

        private TcpClient boyomiClient;
        private string connectedServer;
        private int connectedPort;

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
        }

        public void Free()
        {
            lock (this)
            {
                this.boyomiClient?.Dispose();
                this.boyomiClient = null;
            }
        }

        private string lastText;
        private DateTime lastTextTimestamp;

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public void Speak(
            string text)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (this)
                    {
                        this.SpeakCore(text);
                    }
                }
                catch (Exception ex)
                {
                    this.GetLogger().Error(ex, "棒読みちゃんの読上げで例外が発生しました。");
                }
            });
        }

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        private void SpeakCore(
            string text)
        {
            if (this.lastText == text &&
                (DateTime.Now - this.lastTextTimestamp).TotalSeconds
                <= Settings.Default.GlobalSoundInterval)
            {
                return;
            }

            this.lastText = text;
            this.lastTextTimestamp = DateTime.Now;

            // 棒読みちゃんに接続する
            if (this.ConnectToBoyomi())
            {
                return;
            }

            using (var ns = this.boyomiClient.GetStream())
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

        private bool ConnectToBoyomi()
        {
            var server = Settings.Default.BoyomiServer;
            var port = Settings.Default.BoyomiPort;

            if (string.IsNullOrEmpty(server))
            {
                this.GetLogger().Error("Server name is Empty.");
                return false;
            }

            if (port > 65535 ||
                port < 1)
            {
                this.GetLogger().Error("Port number is Invalid.");
                return false;
            }

            if (server.ToLower() == "localhost")
            {
                server = "127.0.0.1";
            }

            if (this.boyomiClient == null)
            {
                this.boyomiClient = new TcpClient(server, port);
                this.connectedServer = server;
                this.connectedPort = port;
            }
            else
            {
                if (!this.boyomiClient.Connected)
                {
                    this.boyomiClient = new TcpClient(server, port);
                    this.connectedServer = server;
                    this.connectedPort = port;
                }
                else
                {
                    if (this.connectedServer != server ||
                        this.connectedPort != port)
                    {
                        this.boyomiClient.Close();
                        this.boyomiClient.Dispose();

                        this.boyomiClient = new TcpClient(server, port);
                        this.connectedServer = server;
                        this.connectedPort = port;
                    }
                }
            }

            return true;
        }
    }
}
