namespace ACT.TTSYukkuri
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using Advanced_Combat_Tracker;

    public static partial class FF14PluginHelper
    {
        private static object lockObject = new object();
        private static object plugin;
        private static dynamic pluginConfig;
        private static object pluginMemory;
        private static dynamic pluginScancombat;

        public static Process GetFFXIVProcess
        {
            get
            {
                try
                {
                    Initialize();
                    return (Process)pluginConfig?.Process;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<Combatant> GetCombatantList()
        {
            Initialize();

            var result = new List<Combatant>();

            if (plugin == null ||
                GetFFXIVProcess == null ||
                pluginScancombat == null)
            {
                return result;
            }

            dynamic list = pluginScancombat.GetCombatantList();
            foreach (dynamic item in list.ToArray())
            {
                if (item == null)
                {
                    continue;
                }

                var combatant = new Combatant();

                combatant.ID = (uint)item.ID;
                combatant.OwnerID = (uint)item.OwnerID;
                combatant.Job = (int)item.Job;
                combatant.Name = (string)item.Name;
                combatant.type = (byte)item.type;
                combatant.Level = (int)item.Level;
                combatant.CurrentHP = (int)item.CurrentHP;
                combatant.MaxHP = (int)item.MaxHP;
                combatant.CurrentMP = (int)item.CurrentMP;
                combatant.MaxMP = (int)item.MaxMP;
                combatant.CurrentTP = (int)item.CurrentTP;

                result.Add(combatant);
            }

            return result;
        }

        public static List<uint> GetCurrentPartyList(
            out int partyCount)
        {
            Initialize();

            var partyList = new List<uint>();
            partyCount = 0;

            if (plugin == null ||
                GetFFXIVProcess == null ||
                pluginScancombat == null)
            {
                return partyList;
            }

            partyList = pluginScancombat.GetCurrentPartyList(
                out partyCount) as List<uint>;

            return partyList;
        }

        public static void Initialize()
        {
            lock (lockObject)
            {
                if (!ActGlobals.oFormActMain.Visible)
                {
                    return;
                }

                if (plugin == null)
                {
                    foreach (var item in ActGlobals.oFormActMain.ActPlugins)
                    {
                        if (item.pluginFile.Name.ToUpper().Contains("FFXIV_ACT_Plugin.dll".ToUpper()) &&
                            item.lblPluginStatus.Text.ToUpper().Contains("FFXIV Plugin Started.".ToUpper()))
                        {
                            plugin = item.pluginObj;
                            break;
                        }
                    }
                }

                if (plugin != null)
                {
                    FieldInfo fi;

                    if (pluginMemory == null)
                    {
                        fi = plugin.GetType().GetField(
                            "_Memory",
                            BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
                        pluginMemory = fi.GetValue(plugin);
                    }

                    if (pluginMemory == null)
                    {
                        return;
                    }

                    if (pluginConfig == null)
                    {
                        fi = pluginMemory.GetType().GetField(
                            "_config",
                            BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
                        pluginConfig = fi.GetValue(pluginMemory);
                    }

                    if (pluginConfig == null)
                    {
                        return;
                    }

                    if (pluginScancombat == null)
                    {
                        fi = pluginConfig.GetType().GetField(
                            "ScanCombatants",
                            BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
                        pluginScancombat = fi.GetValue(pluginConfig);
                    }
                }
            }
        }
    }

    public class Combatant
    {
        public int CurrentHP;
        public int CurrentMP;
        public int CurrentTP;
        public uint ID;
        public int Job;
        public int Level;
        public int MaxHP;
        public int MaxMP;
        public string Name;
        public int Order;
        public uint OwnerID;
        public byte type;
    }
}
