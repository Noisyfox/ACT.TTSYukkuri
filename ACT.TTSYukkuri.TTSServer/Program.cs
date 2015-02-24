namespace ACT.TTSYukkuri.TTSServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;

    using ACT.TTSYukkuri.TTSServer.Core;
    using ACT.TTSYukkuri.TTSServer.Properties;
    using CeVIO.Talk.RemoteService;
    using NAudio.Wave;

    public static class Program
    {
        private const string YukkuriDll = @"AquesTalk.dll";

        private static object lockObject = new object();

        private static Talker talker;

        private static IpcServerChannel channel;

        [DllImport(YukkuriDll)]
        private static extern IntPtr AquesTalk_Synthe(string koe, ushort iSpeed, ref uint size);

        [DllImport(YukkuriDll)]
        private static extern void AquesTalk_FreeWave(IntPtr wave);

        public static void Main(string[] args)
        {
            channel = new IpcServerChannel("TTSYukkuriChannel");

            ChannelServices.RegisterChannel(channel, true);

            var message = new TTSMessage();
            message.OnSpeak += message_OnSpeak;
            message.OnGetSasaraSettings += message_OnGetSasaraSettings;
            message.OnSetSasaraSettings += message_OnSetSasaraSettings;
            message.OnStartSasara += message_OnStartSasara;
            message.OnCloseSasara += message_OnCloseSasara;
            message.OnEnd += message_OnEnd;

            RemotingServices.Marshal(message, "message", typeof(TTSMessage));

            // 終了を待つ
            Console.ReadLine();
        }

        private static void message_OnEnd()
        {
            message_OnCloseSasara();

            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel = null;
            }
        }

        private static void message_OnSpeak(TTSMessage.SpeakEventArg e)
        {
            switch (e.TTSType)
            {
                case TTSTEngineType.Yukkuri:
                    SpeakByYukkuri(e);
                    break;

                case TTSTEngineType.CeVIO:
                    SpeakBySasara(e);
                    break;
            }
        }

        private static void SpeakByYukkuri(TTSMessage.SpeakEventArg e)
        {
            lock (lockObject)
            {
                IntPtr wavePtr = IntPtr.Zero;

                try
                {
                    if (string.IsNullOrWhiteSpace(e.TextToSpeack))
                    {
                        return;
                    }

                    // テキストを音声データに変換する
                    uint size = 0;
                    wavePtr = AquesTalk_Synthe(
                        e.TextToSpeack,
                        (ushort)e.SpeakSpeed,
                        ref size);

                    if (wavePtr == IntPtr.Zero ||
                        size <= 0)
                    {
                        return;
                    }

                    // 生成したwaveデータを読み出す
                    var buff = new byte[size];
                    Marshal.Copy(wavePtr, buff, 0, (int)size);

                    using (var fs = new FileStream(e.WaveFile, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(buff, 0, buff.Length);
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

        private static void SpeakBySasara(TTSMessage.SpeakEventArg e)
        {
            lock (lockObject)
            {
                if (string.IsNullOrWhiteSpace(e.TextToSpeack))
                {
                    return;
                }

                message_OnStartSasara();

                var tempWave = Path.GetTempFileName();

                talker.OutputWaveToFile(
                    e.TextToSpeack,
                    tempWave);

                // ささらは音量が小さめなので増幅する
                using (var reader = new WaveFileReader(tempWave))
                {
                    var prov = new VolumeWaveProvider16(reader);
                    prov.Volume = Settings.Default.SasaraGain;

                    WaveFileWriter.CreateWaveFile(
                        e.WaveFile,
                        prov);
                }

                if (File.Exists(tempWave))
                {
                    File.Delete(tempWave);
                }
            }
        }

        private static void message_OnStartSasara()
        {
            if (!ServiceControl.IsHostStarted)
            {
                ServiceControl.StartHost(false);
            }

            if (talker == null)
            {
                talker = new Talker();
            }
        }

        private static void message_OnCloseSasara()
        {
            if (ServiceControl.IsHostStarted)
            {
                ServiceControl.CloseHost();
            }

            if (talker != null)
            {
                talker = null;
            }
        }

        private static SasaraSettings message_OnGetSasaraSettings()
        {
            message_OnStartSasara();

            var settings = new SasaraSettings();

            settings.Volume = talker.Volume;
            settings.Speed = talker.Speed;
            settings.Tone = talker.Tone;
            settings.Alpha = talker.Alpha;
            settings.Cast = talker.Cast;
            settings.AvailableCasts = Talker.AvailableCasts;

            var compornents = new List<SasaraTalkerComponent>();
            for (int i = 0; i < talker.Components.Count; i++)
            {
                compornents.Add(new SasaraTalkerComponent()
                {
                    Id = talker.Components[i].Id,
                    Name = talker.Components[i].Name,
                    Value = talker.Components[i].Value,
                });
            }

            settings.Components = compornents.ToArray();

            return settings;
        }

        private static void message_OnSetSasaraSettings(TTSMessage.SasaraSettingsEventArg e)
        {
            message_OnStartSasara();

            if (string.IsNullOrWhiteSpace(talker.Cast) &&
                Talker.AvailableCasts.Length > 0)
            {
                talker.Cast = Talker.AvailableCasts[0];
            }

            if (talker.Cast != e.Settings.Cast ||
                talker.Volume != e.Settings.Volume ||
                talker.Speed != e.Settings.Speed ||
                talker.Tone != e.Settings.Tone ||
                talker.Alpha != e.Settings.Alpha)
            {
                talker.Cast = e.Settings.Cast;
                talker.Volume = e.Settings.Volume;
                talker.Speed = e.Settings.Speed;
                talker.Tone = e.Settings.Tone;
                talker.Alpha = e.Settings.Alpha;
            }

            if (e.Settings.Components != null)
            {
                foreach (var c in e.Settings.Components)
                {
                    var t = talker.Components
                        .Where(x => x.Id == c.Id)
                        .FirstOrDefault();

                    if (t != null)
                    {
                        if (t.Value != c.Value)
                        {
                            t.Value = c.Value;
                        }
                    }
                }
            }
        }
    }
}
