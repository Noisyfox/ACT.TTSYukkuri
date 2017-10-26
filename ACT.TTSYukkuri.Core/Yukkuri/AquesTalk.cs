using System;
using System.IO;
using System.Runtime.InteropServices;
using FFXIV.Framework.Common;

namespace ACT.TTSYukkuri.Yukkuri
{
    public class AquesTalk
    {
        /// <summary>
        /// 開発者ライセンスキー
        /// </summary>
        /// <remarks>
        /// ここにAquesTalk10の開発者ライセンスキーをセットします。</remarks>
        private static readonly string DeveloperKey = "#DEVELOPER_KEY_IS_HERE#";

        #region Singleton

        private static AquesTalk instance = new AquesTalk();
        public static AquesTalk Instance => instance;

        #endregion Singleton

        private const string YukkuriLibName = "AquesTalk";

        private static readonly string YukkuriDllName = Path.Combine(
            PluginCore.Instance.PluginDirectory,
            $@"Yukkuri\{YukkuriLibName}.dll");

        private UnmanagedLibrary yukkuriLib;
        private AquesTalk_SetDevKey SetDevKeyDelegate;
        private AquesTalk_Synthe SynthesizeDelegate;
        private AquesTalk_Synthe_Utf16 SynthesizeUTF16Delegate;
        private AquesTalk_FreeWave FreeWaveDelegate;

        private delegate int AquesTalk_SetDevKey(string key);

        private delegate IntPtr AquesTalk_Synthe(ref AQTK_VOICE pParam, string koe, ref uint size);

        private delegate IntPtr AquesTalk_Synthe_Utf16(ref AQTK_VOICE pParam, string koe, ref uint size);

        private delegate void AquesTalk_FreeWave(IntPtr wave);

        public void Load()
        {
            if (this.yukkuriLib == null)
            {
                this.yukkuriLib = new UnmanagedLibrary(YukkuriDllName);
                this.IsLoadedAppKey = false;
            }

            if (this.yukkuriLib == null)
            {
                return;
            }

            if (this.SetDevKeyDelegate == null)
            {
                this.SetDevKeyDelegate =
                    this.yukkuriLib.GetUnmanagedFunction<AquesTalk_SetDevKey>(nameof(AquesTalk_SetDevKey));
            }

            if (this.SynthesizeDelegate == null)
            {
                this.SynthesizeDelegate =
                    this.yukkuriLib.GetUnmanagedFunction<AquesTalk_Synthe>(nameof(AquesTalk_Synthe));
            }

            if (this.SynthesizeUTF16Delegate == null)
            {
                this.SynthesizeUTF16Delegate =
                    this.yukkuriLib.GetUnmanagedFunction<AquesTalk_Synthe_Utf16>(nameof(AquesTalk_Synthe_Utf16));
            }

            if (this.FreeWaveDelegate == null)
            {
                this.FreeWaveDelegate =
                    this.yukkuriLib.GetUnmanagedFunction<AquesTalk_FreeWave>(nameof(AquesTalk_FreeWave));
            }
        }

        public bool IsLoadedAppKey { get; private set; }

        public void Free()
        {
            if (this.yukkuriLib != null)
            {
                this.IsLoadedAppKey = false;

                this.SetDevKeyDelegate = null;
                this.SynthesizeDelegate = null;
                this.SynthesizeUTF16Delegate = null;
                this.FreeWaveDelegate = null;

                this.yukkuriLib.Dispose();
                this.yukkuriLib = null;
            }
        }

        /// <summary>
        /// アプリケーションキーをセットする
        /// </summary>
        /// <returns>
        /// status</returns>
        public bool SetAppKey()
        {
            var result = this.SetDevKeyDelegate?.Invoke(DeveloperKey);
            if (result != 0)
            {
                this.IsLoadedAppKey = false;
                return false;
            }

            this.IsLoadedAppKey = true;
            return true;
        }

        public void TextToWave(
            string textToSpeak,
            string waveFileName,
            AQTK_VOICE voice)
        {
            if (string.IsNullOrWhiteSpace(textToSpeak))
            {
                return;
            }

            if (this.SynthesizeDelegate == null ||
                this.SynthesizeUTF16Delegate == null ||
                this.FreeWaveDelegate == null)
            {
                return;
            }

            var wavePtr = IntPtr.Zero;

            try
            {
                // テキストを音声データに変換する
                uint size = 0;
                wavePtr = this.SynthesizeDelegate.Invoke(
                    ref voice,
                    textToSpeak,
                    ref size);

                if (wavePtr == IntPtr.Zero ||
                    size <= 0)
                {
                    return;
                }

                FileHelper.CreateDirectory(waveFileName);

                // 生成したwaveデータを読み出す
                var buff = new byte[size];
                Marshal.Copy(wavePtr, buff, 0, (int)size);
                using (var fs = new FileStream(waveFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buff, 0, buff.Length);
                }
            }
            finally
            {
                if (wavePtr != IntPtr.Zero)
                {
                    this.FreeWaveDelegate.Invoke(wavePtr);
                }
            }
        }
    }
}
