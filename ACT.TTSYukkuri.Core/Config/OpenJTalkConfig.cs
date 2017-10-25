using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config
{
    [Serializable]
    public class OpenJTalkConfig :
        BindableBase
    {
        private string voice = "mei_normal.htsvoice";
        public int gain = 100;
        public int volume = 100;
        public int speed = 100;
        public int pitch = 0;

        public void SetDefault()
        {
            var defaultConfig = new OpenJTalkConfig();
            this.Gain = defaultConfig.Gain;
            this.Volume = defaultConfig.Volume;
            this.Speed = defaultConfig.Speed;
            this.Pitch = defaultConfig.Pitch;
        }

        public string Voice
        {
            get => this.voice;
            set => this.SetProperty(ref this.voice, value);
        }

        public int Gain
        {
            get => this.gain;
            set => this.SetProperty(ref this.gain, value);
        }

        public int Volume
        {
            get => this.volume;
            set => this.SetProperty(ref this.volume, value);
        }

        public int Speed
        {
            get => this.speed;
            set => this.SetProperty(ref this.speed, value);
        }

        public int Pitch
        {
            get => this.pitch;
            set => this.SetProperty(ref this.pitch, value);
        }

        [XmlIgnore]
        public string OpenJTalkDirectory
        {
            get
            {
                // ACTのパスを取得する
                var asm = Assembly.GetEntryAssembly();
                if (asm != null)
                {
                    var actDirectory = Path.GetDirectoryName(asm.Location);
                    var resourcesUnderAct = Path.Combine(actDirectory, @"OpenJTalk");

                    if (Directory.Exists(resourcesUnderAct))
                    {
                        return resourcesUnderAct;
                    }
                }

                // 自身の場所を取得する
                var selfDirectory = PluginCore.Instance.PluginDirectory ?? string.Empty;
                var resourcesUnderThis = Path.Combine(selfDirectory, @"OpenJTalk");

                if (Directory.Exists(resourcesUnderThis))
                {
                    return resourcesUnderThis;
                }

                return string.Empty;
            }
        }

        public OpenJTalkVoice[] EnumlateVoice()
        {
            var list = new List<OpenJTalkVoice>();

            var openTalk = this.OpenJTalkDirectory;

            if (string.IsNullOrWhiteSpace(openTalk))
            {
                return list.ToArray();
            }

            var voice = Path.Combine(
                openTalk,
                "voice");

            if (Directory.Exists(voice))
            {
                foreach (var item in Directory.GetFiles(voice, "*.htsvoice")
                    .OrderBy(x => x)
                    .ToArray())
                {
                    list.Add(new OpenJTalkVoice()
                    {
                        File = item
                    });
                }
            }

            return list.ToArray();
        }

        public override string ToString() =>
            $"{nameof(this.Voice)}:{this.Voice}," +
            $"{nameof(this.Gain)}:{this.Gain}," +
            $"{nameof(this.Volume)}:{this.Volume}," +
            $"{nameof(this.Speed)}:{this.Speed}," +
            $"{nameof(this.Pitch)}:{this.Pitch}";
    }

    [Serializable]
    public class OpenJTalkVoice
    {
        public string Value => Path.GetFileName(this.File);

        public string Name => Path.GetFileName(this.File);

        public string File { get; set; }

        public override string ToString() => this.Name;
    }
}
