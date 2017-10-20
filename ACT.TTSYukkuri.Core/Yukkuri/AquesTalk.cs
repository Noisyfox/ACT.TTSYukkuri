using System;
using System.IO;
using System.Runtime.InteropServices;
using FFXIV.Framework.Common;

namespace ACT.TTSYukkuri.Yukkuri
{
    public class AquesTalk
    {
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
            if (!NetiveMethods.IsModuleLoaded(YukkuriLibName))
            {
                this.yukkuriLib = new UnmanagedLibrary(YukkuriDllName);
            }

            if (this.yukkuriLib != null)
            {
                if (this.SetDevKeyDelegate == null)
                {
                    this.SetDevKeyDelegate =
                        this.yukkuriLib.GetUnmanagedFunction<AquesTalk_SetDevKey>(nameof(AquesTalk_SetDevKey));
                    /*
                                        // 開発ライセンスを登録する
                                        this.SetDevKeyDelegate(Resources.AquesTalkDevKey);
                    */
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
        }

        public void Free()
        {
            if (this.yukkuriLib != null)
            {
                this.SetDevKeyDelegate = null;
                this.SynthesizeDelegate = null;
                this.SynthesizeUTF16Delegate = null;
                this.FreeWaveDelegate = null;

                this.yukkuriLib.Dispose();
                this.yukkuriLib = null;
            }
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

        #region Native Methods

        private static class NetiveMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetModuleHandle(string moduleName);

            /// <summary>
            /// Check whether or not the specified module is loaded in the
            /// current process.
            /// </summary>
            /// <param name="moduleName">the module name</param>
            /// <returns>
            /// The function returns true if the specified module is loaded in
            /// the current process. If the module is not loaded, the function
            /// returns false.
            /// </returns>
            public static bool IsModuleLoaded(string moduleName)
            {
                // Get the module in the process according to the module name.
                IntPtr hMod = GetModuleHandle(moduleName);
                return (hMod != IntPtr.Zero);
            }
        }

        #endregion Native Methods
    }
}
