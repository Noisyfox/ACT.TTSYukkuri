namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class YukkuriController
    {
        #region Singleton

        private static YukkuriController instance = new YukkuriController();

        public static YukkuriController Default => instance;

        #endregion Singleton

        private readonly string DllName = "AquesTalk.dll";

        private UnmanagedLibrary aquesTalkLib;
        private AquesTalk_FreeWave FreeWaveDelegate;
        private AquesTalk_Synthe SynthesizeDelegate;

        private delegate void AquesTalk_FreeWave(IntPtr wave);

        private delegate IntPtr AquesTalk_Synthe(string koe, ushort iSpeed, ref uint size);

        public void Free()
        {
            if (this.aquesTalkLib != null)
            {
                this.aquesTalkLib.Dispose();
                this.aquesTalkLib = null;
            }
        }

        public void OutputWaveToFile(
            string textToSpeak,
            ushort speed,
            string waveFile)
        {
            if (string.IsNullOrWhiteSpace(textToSpeak))
            {
                return;
            }

            this.Initialize();

            if (this.SynthesizeDelegate == null ||
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
                    textToSpeak,
                    speed,
                    ref size);

                if (wavePtr == IntPtr.Zero ||
                    size <= 0)
                {
                    return;
                }

                // 生成したwaveデータを読み出す
                var buff = new byte[size];
                Marshal.Copy(wavePtr, buff, 0, (int)size);

                using (var fs = new FileStream(waveFile, FileMode.Create, FileAccess.Write))
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

        private void Initialize()
        {
            if (!YukkuriController.IsModuleLoaded("AquesTalk"))
            {
                this.aquesTalkLib = new UnmanagedLibrary(DllName);
            }

            if (this.aquesTalkLib != null)
            {
                if (this.SynthesizeDelegate == null)
                {
                    this.SynthesizeDelegate =
                        this.aquesTalkLib.GetUnmanagedFunction<AquesTalk_Synthe>("AquesTalk_Synthe");
                }

                if (this.FreeWaveDelegate == null)
                {
                    this.FreeWaveDelegate =
                        this.aquesTalkLib.GetUnmanagedFunction<AquesTalk_FreeWave>("AquesTalk_FreeWave");
                }
            }
        }

        #region IsModuleLoaded

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string moduleName);

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
        private static bool IsModuleLoaded(string moduleName)
        {
            // Get the module in the process according to the module name.
            IntPtr hMod = GetModuleHandle(moduleName);
            return (hMod != IntPtr.Zero);
        }

        #endregion IsModuleLoaded
    }
}
