namespace HPBarOverlay.SoundPlayer
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

                var sound = args[0];
                var delete = false;

                if (args.Length >= 2)
                {
                    bool.TryParse(args[1], out delete);
                }

                using (var player = new SoundPlayer(sound))
                {
                    player.PlaySync();
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
