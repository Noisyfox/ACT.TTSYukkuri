namespace ACT.TTSYukkuri.Yukkuri
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ゆっくりスピーチコントローラ
    /// </summary>
    public class YukkuriSpeechController :
        SpeechControllerBase,
        ISpeechController
    {
        /// <summary>
        /// しゃべるスピード
        /// </summary>
        private const ushort SpeechSpeed = 110;

        /// <summary>
        /// AquesTalk_Synthe
        /// </summary>
        /// <param name="koe">読み上げるテスト</param>
        /// <param name="iSpeed">スピード</param>
        /// <param name="size">生成したwaveデータのサイズ</param>
        /// <returns>生成した音声waveデータ</returns>
        [DllImport(@"aqtk1-win\lib\AquesTalk.dll")]
        private static extern IntPtr AquesTalk_Synthe(string koe, ushort iSpeed, ref uint size);

        /// <summary>
        /// AquesTalk_FreeWave
        /// </summary>
        /// <param name="wave">開放する音声waveデータ</param>
        [DllImport(@"aqtk1-win\lib\AquesTalk.dll")]
        private static extern void AquesTalk_FreeWave(IntPtr wave);

        /// <summary>
        /// テキストを読み上げる
        /// </summary>
        /// <param name="text">読み上げるテキスト</param>
        public override void Speak(
            string text)
        {
            IntPtr wavePtr = IntPtr.Zero;

            try
            {
                // テキストを音声データに変換する
                uint size = 0;
                wavePtr = AquesTalk_Synthe(
                    text,
                    SpeechSpeed,
                    ref size);

                if (wavePtr == IntPtr.Zero)
                {
                    return;
                }

                // 生成したwaveデータを読み出す
                var buff = new byte[size];
                Marshal.Copy(wavePtr, buff, 0, (int)size);

                // 再生する
                using (var ms = new MemoryStream(buff))
                {
                    SoundPlayerRapper.Play(ms);
                }
            }
            finally
            {
                if (wavePtr != IntPtr.Zero)
                {
                    AquesTalk_FreeWave(wavePtr);
                }
            }
        }
    }
}
