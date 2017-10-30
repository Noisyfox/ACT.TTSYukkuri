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

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public void Speak(
            string text)
        {
            try
            {
                // 棒読みちゃんに接続する
                this.ConnectToBoyomi();

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
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "ACT.TTSYukkuri 棒読みちゃんの読上げで例外が発生しました");
            }
        }

        private void ConnectToBoyomi()
        {
            lock (this)
            {
                var server = TTSYukkuriConfig.Default.BoyomiServer;
                var port = TTSYukkuriConfig.Default.BoyomiPort;

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
            }
        }
    }
}
