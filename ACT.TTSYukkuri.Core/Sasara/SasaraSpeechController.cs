using System.IO;
using ACT.TTSYukkuri.Config;
using FFXIV.Framework.TTS.Common;

namespace ACT.TTSYukkuri.Sasara
{
    /// <summary>
    /// さとうささらスピーチコントローラ
    /// </summary>
    public class SasaraSpeechController :
        ISpeechController
    {
        /// <summary>
        /// 初期化する
        /// </summary>
        public void Initialize()
        {
            TTSYukkuriConfig.Default.SasaraSettings.SetToRemote();
        }

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
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            // 現在の条件をハッシュ化してWAVEファイル名を作る
            var wave = this.GetCacheFileName(
                TTSYukkuriConfig.Default.TTS,
                text,
                TTSYukkuriConfig.Default.SasaraSettings.ToString());

            lock (this)
            {
                if (!File.Exists(wave))
                {
                    // 音声waveファイルを生成する
                    RemoteTTSClient.Instance.TTSModel.TextToWave(
                        TTSTypes.CeVIO,
                        text,
                        wave,
                        0,
                        TTSYukkuriConfig.Default.SasaraSettings.Gain);
                }
            }

            // 再生する
            SoundPlayerWrapper.Play(wave);
        }
    }
}
