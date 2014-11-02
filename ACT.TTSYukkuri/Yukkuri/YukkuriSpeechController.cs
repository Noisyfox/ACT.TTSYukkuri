namespace ACT.TTSYukkuri.Yukkuri
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;

    using ACT.TTSYukkuri.Config;

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
        /// TTSの設定Panel
        /// </summary>
        public override UserControl TTSSettingsPanel
        {
            get
            {
                return YukkuriSettingsPanel.Default;
            }
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public override void Initialize()
        {
            // NO-OP
        }

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
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                // よみがなに変換する
                var textByYomigana = this.ConvertYomigana(text);

                // テキストを音声データに変換する
                uint size = 0;
                wavePtr = AquesTalk_Synthe(
                    textByYomigana,
                    SpeechSpeed,
                    ref size);

                if (wavePtr == IntPtr.Zero ||
                    size <= 0)
                {
                    return;
                }

                // 生成したwaveデータを読み出す
                var buff = new byte[size];
                Marshal.Copy(wavePtr, buff, 0, (int)size);

                // 再生する
                using (var ms = new MemoryStream(buff))
                {
                    if (!TTSYukkuriConfig.Default.EnabledYukkuriVolumeSetting)
                    {
                        SoundPlayerWrapper.Play(ms);
                    }
                    else
                    {
                        SoundPlayerWrapper.Play(ms, TTSYukkuriConfig.Default.YukkuriVolume);
                    }
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

        /// <summary>
        /// よみがなに変換する
        /// </summary>
        /// <param name="textToConvert">変換するテキスト</param>
        /// <returns>よみがなに変換したテキスト</returns>
        private string ConvertYomigana(
            string textToConvert)
        {
            var yomigana = textToConvert;

#if false
            IFELanguage ifelang = null;

            try
            {
                ifelang = Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan")) as IFELanguage;

                var hr = ifelang.Open();
                if (hr != 0)
                {
                    return yomigana;
                }

                string t;
                hr = ifelang.GetPhonetic(textToConvert, 1, -1, out t);
                if (hr != 0)
                {
                    return yomigana;
                }

                yomigana = t;
            }
            finally
            {
                if (ifelang != null)
                {
                    ifelang.Close();
                }
            }
#endif

            // スペースを置き換える
            yomigana = yomigana.Replace(" ", "、");
            yomigana = yomigana.Replace("　", "、");

            // 感嘆符を置き換える
            yomigana = yomigana.Replace("!", "。");
            yomigana = yomigana.Replace("！", "。");

            // アルファベットを置き換える
            var regex2 = new Regex(@"[a-zA-Zａ-ｚＡ-Ｚ]+");
            yomigana = regex2.Replace(
                yomigana,
                (match) =>
                {
                    var replacement = match.Value;
                    replacement = Strings.StrConv(replacement, VbStrConv.Narrow);
                    replacement = Strings.StrConv(replacement, VbStrConv.Uppercase);
                    return replacement;
                });

            yomigana = yomigana.Replace("A", "あるふぁ");
            yomigana = yomigana.Replace("B", "ぶらぼー");
            yomigana = yomigana.Replace("C", "ちゃーりー");
            yomigana = yomigana.Replace("D", "でるた");
            yomigana = yomigana.Replace("E", "えこー");
            yomigana = yomigana.Replace("F", "ふぉっくす");
            yomigana = yomigana.Replace("G", "ごるふ");
            yomigana = yomigana.Replace("H", "ほてる");
            yomigana = yomigana.Replace("I", "いんど");
            yomigana = yomigana.Replace("J", "じゅりえっと");
            yomigana = yomigana.Replace("K", "きろ");
            yomigana = yomigana.Replace("L", "りま");
            yomigana = yomigana.Replace("M", "まいく");
            yomigana = yomigana.Replace("N", "のーべんばー");
            yomigana = yomigana.Replace("O", "おすかー");
            yomigana = yomigana.Replace("P", "ぱぱ");
            yomigana = yomigana.Replace("Q", "きゅーべっく");
            yomigana = yomigana.Replace("R", "ろめお");
            yomigana = yomigana.Replace("S", "しえら");
            yomigana = yomigana.Replace("T", "たんご");
            yomigana = yomigana.Replace("U", "ゆにふぉーむ");
            yomigana = yomigana.Replace("V", "びくたー");
            yomigana = yomigana.Replace("W", "ういすきー");
            yomigana = yomigana.Replace("X", "えっくすれい");
            yomigana = yomigana.Replace("Y", "やんきー");
            yomigana = yomigana.Replace("Z", "ずーるー");

            // 数字を音声記号に置き換える
            yomigana = yomigana.Replace("０", "0");
            yomigana = yomigana.Replace("１", "1");
            yomigana = yomigana.Replace("２", "2");
            yomigana = yomigana.Replace("３", "3");
            yomigana = yomigana.Replace("４", "4");
            yomigana = yomigana.Replace("５", "5");
            yomigana = yomigana.Replace("６", "6");
            yomigana = yomigana.Replace("７", "7");
            yomigana = yomigana.Replace("８", "8");
            yomigana = yomigana.Replace("９", "9");

            var regex1 = new Regex(@"\d+");
            yomigana = regex1.Replace(
                yomigana,
                (match) =>
                {
                    return "<NUMK VAL=" + match.Value + ">";
                });

            return yomigana;
        }
    }

    /// <summary>
    /// IFELanguage Interface
    /// </summary>
    [ComImport]
    [Guid("019F7152-E6DB-11d0-83C3-00C04FDDB82E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFELanguage
    {
        int Open();
        int Close();
        int GetJMorphResult(uint dwRequest, uint dwCMode, int cwchInput, [MarshalAs(UnmanagedType.LPWStr)] string pwchInput, IntPtr pfCInfo, out object ppResult);
        int GetConversionModeCaps(ref uint pdwCaps);
        int GetPhonetic([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
        int GetConversion([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
    }
}
