namespace ACT.TTSYukkuri.SoundPlayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NAudio.Wave;

    /// <summary>
    /// NAudioプレイヤー
    /// </summary>
    public partial class NAudioPlayer
    {
        /// <summary>
        /// デバイスごとの再生デバイスリスト
        /// </summary>
        private static Dictionary<Guid, DirectSoundOut> players = new Dictionary<Guid, DirectSoundOut>();

        /// <summary>
        /// 再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevices()
        {
            var list = new List<PlayDevice>();

            var i = 0;
            foreach (var device in DirectSoundOut.Devices)
            {
                list.Add(new PlayDevice()
                {
                    Guid = device.Guid,
                    Name = device.ModuleName,
                    Description = device.Description,
                });

                i++;
            }

            return list;
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceID">再生デバイスID</param>
        /// <param name="waveFile">wavファイル</param>
        /// <param name="isDelete">再生後に削除する</param>
        public static void Play(
            Guid deviceID,
            string waveFile,
            bool isDelete)
        {
            Play(
                deviceID,
                waveFile,
                isDelete,
                100);
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="deviceNo">再生デバイスID</param>
        /// <param name="waveFile">wavファイル</param>
        /// <param name="isDelete">再生後に削除する</param>
        /// <param name="volume">ボリューム</param>
        public static void Play(
            Guid deviceID,
            string waveFile,
            bool isDelete,
            int volume)
        {
            var player = players.ContainsKey(deviceID) ?
                players[deviceID] :
                new DirectSoundOut(deviceID, 200);

            var r = new AudioFileReader(waveFile);

            player.Volume = ((float)volume / 100f);
            player.Init(r);
            player.PlaybackStopped += (s, e) =>
            {
                r.Dispose();

                if (isDelete)
                {
                    File.Delete(waveFile);
                }
            };

            // 再生する
            player.Play();
        }

        /// <summary>
        /// プレイヤを開放する
        /// </summary>
        public static void DisposePlayers()
        {
            if (players != null)
            {
                foreach (var p in players.Values)
                {
                    p.Dispose();
                }

                players.Clear();
            }
        }
    }

    /// <summary>
    /// 再生デバイス
    /// </summary>
    public class PlayDevice
    {
        /// <summary>
        /// デバイスのGUID
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// デバイス名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns>デバイスの名称</returns>
        public override string ToString()
        {
            return this.Description;
        }
    }
}
