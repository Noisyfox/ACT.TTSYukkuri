namespace ACT.TTSYukkuri.SoundPlayer
{
    using System.Collections.Generic;
    using System.IO;

    using NAudio.Wave;

    /// <summary>
    /// NAudioプレイヤー
    /// </summary>
    public partial class NAudioPlayer
    {
        /// <summary>
        /// 再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevices()
        {
            var list = new List<PlayDevice>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);
                list.Add(new PlayDevice()
                {
                    Number = i,
                    Name = capabilities.ProductName
                });
            }

            return list;
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceNo">再生デバイス番号</param>
        /// <param name="waveFile">wavファイル</param>
        /// <param name="isDelete">再生後に削除する</param>
        public static void Play(
            int deviceNo,
            string waveFile,
            bool isDelete)
        {
            Play(
                deviceNo,
                waveFile,
                isDelete,
                100);
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceNo">再生デバイス番号</param>
        /// <param name="waveFile">wavファイル</param>
        /// <param name="isDelete">再生後に削除する</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            int deviceNo,
            string waveFile,
            bool isDelete,
            int volume)
        {
            var player = new WaveOut()
            {
                DeviceNumber = deviceNo,
                DesiredLatency = 200,
                Volume = ((float)volume / 100f),
            } as IWavePlayer;

            var wr = new WaveFileReader(waveFile);

            player.Init(wr);
            player.PlaybackStopped += (s, e) =>
            {
                (s as IWavePlayer).Dispose();
                wr.Dispose();

                if (isDelete)
                {
                    File.Delete(waveFile);
                }
            };

            // 再生する
            player.Play();
        }
    }

    /// <summary>
    /// 再生デバイス
    /// </summary>
    public class PlayDevice
    {
        /// <summary>
        /// デバイスNo
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// デバイス名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns>デバイスの名称</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
