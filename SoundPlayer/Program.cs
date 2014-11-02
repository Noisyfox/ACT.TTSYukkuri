namespace SoundPlayer
{
    using System;
    using System.IO;
    using System.Media;

    /// <summary>
    /// エントリポイント
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    return;
                }

                // 再生するファイル名を取得する
                var sound = args[0];

                // 再生したファイルを削除するかどうかを指定するフラグを取得する
                var delete = false;
                if (args.Length >= 2)
                {
                    if (!bool.TryParse(args[1], out delete))
                    {
                        delete = false;
                    }
                }

                // 音量を取得する
                var volume = -1;
                if (args.Length >= 3)
                {
                    if (!int.TryParse(args[2], out volume))
                    {
                        volume = -1;
                    }
                }

                if (volume == -1)
                {
                    using (var player = new SoundPlayer(sound))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    using (var soundStream = new WaveStream(File.OpenRead(sound)))
                    {
                        if (volume <= 0)
                        {
                            volume = 0;
                        }

                        if (volume >= 100)
                        {
                            volume = 100;
                        }

                        soundStream.Volume = volume;

                        using (var player = new SoundPlayer(soundStream))
                        {
                            player.PlaySync();
                        }
                    }
                }

                if (delete)
                {
                    File.Delete(sound);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
