using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.TTSYukkuri
{
    public class Plugin :
        IActPluginV1,
        ISpeak
    {
        #region Singleton

        private static Plugin instance;

        public static Plugin Instance => instance;

        #endregion Singleton

        public Plugin()
        {
            instance = this;

            // このDLLの配置場所とACT標準のPluginディレクトリも解決の対象にする
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                try
                {
                    var asm = new AssemblyName(e.Name);

                    var plugin = ActGlobals.oFormActMain.PluginGetSelfData(this);
                    if (plugin != null)
                    {
                        var thisDirectory = plugin.pluginFile.DirectoryName;
                        var path1 = Path.Combine(thisDirectory, asm.Name + ".dll");
                        if (File.Exists(path1))
                        {
                            return Assembly.LoadFrom(path1);
                        }
                    }

                    var pluginDirectory = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Advanced Combat Tracker\Plugins");

                    var path = Path.Combine(pluginDirectory, asm.Name + ".dll");

                    if (File.Exists(path))
                    {
                        return Assembly.LoadFrom(path);
                    }
                }
                catch (Exception ex)
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(
                        ex,
                        "ACT.TTSYukkuri Assemblyの解決で例外が発生しました");
                }

                return null;
            };
        }

        public void DeInitPlugin()
        {
            PluginCore.Instance?.DeInitPlugin();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InitPlugin(
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            PluginCore.Instance.InitPlugin(
                this,
                pluginScreenSpace,
                pluginStatusText);
        }

        #region ISpeak

        public void Speak(string textToSpeak) =>
            PluginCore.Instance.Speak(textToSpeak);

        #endregion ISpeak
    }
}
