using System;
using System.Linq;
using System.Xml.Serialization;
using ACT.TTSYukkuri.Yukkuri;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config
{
    [Serializable]
    public class YukkuriConfig :
        BindableBase
    {
        private AQPresets preset = AQPresets.F1;
        private AQBaseVoices baseVoice = (AQBaseVoices)AQVoicePresets.F1.bas;
        private int speed = AQVoicePresets.F1.spd;
        private int volume = AQVoicePresets.F1.vol;
        private int pitch = AQVoicePresets.F1.pit;
        private int accent = AQVoicePresets.F1.acc;
        private int lmd = AQVoicePresets.F1.lmd;
        private int fsc = AQVoicePresets.F1.fsc;

        public void LoadPreset(
            AQPresets preset)
        {
            var presetParam = preset.GetPreset();
            this.baseVoice = (AQBaseVoices)presetParam.bas;
            this.speed = presetParam.spd;
            this.volume = presetParam.vol;
            this.pitch = presetParam.pit;
            this.accent = presetParam.acc;
            this.lmd = presetParam.lmd;
            this.fsc = presetParam.fsc;

            this.RaisePropertyChanged(nameof(this.BaseVoice));
            this.RaisePropertyChanged(nameof(this.Speed));
            this.RaisePropertyChanged(nameof(this.Volume));
            this.RaisePropertyChanged(nameof(this.Pitch));
            this.RaisePropertyChanged(nameof(this.Accent));
            this.RaisePropertyChanged(nameof(this.LMD));
            this.RaisePropertyChanged(nameof(this.FSC));
        }

        private bool IsCutom()
        {
            var nowParam = this.ToParameter();
            var any = AQVoicePresets.Presets.Any(x =>
                x.Parameter.bas == nowParam.bas &&
                x.Parameter.spd == nowParam.spd &&
                x.Parameter.vol == nowParam.vol &&
                x.Parameter.pit == nowParam.pit &&
                x.Parameter.acc == nowParam.acc &&
                x.Parameter.lmd == nowParam.lmd &&
                x.Parameter.fsc == nowParam.fsc);

            return !any;
        }

        [XmlIgnore]
        public AQPresets Preset
        {
            get => this.preset;
            set
            {
                if (this.SetProperty(ref this.preset, value))
                {
                    if (this.preset == AQPresets.Custom)
                    {
                        return;
                    }

                    this.LoadPreset(this.preset);
                }
            }
        }

        public AQBaseVoices BaseVoice
        {
            get => this.baseVoice;
            set
            {
                if (this.SetProperty(ref this.baseVoice, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int Speed
        {
            get => this.speed;
            set
            {
                if (this.SetProperty(ref this.speed, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int Volume
        {
            get => this.volume;
            set
            {
                if (this.SetProperty(ref this.volume, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int Pitch
        {
            get => this.pitch;
            set
            {
                if (this.SetProperty(ref this.pitch, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int Accent
        {
            get => this.accent;
            set
            {
                if (this.SetProperty(ref this.accent, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int LMD
        {
            get => this.lmd;
            set
            {
                if (this.SetProperty(ref this.lmd, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public int FSC
        {
            get => this.fsc;
            set
            {
                if (this.SetProperty(ref this.fsc, value))
                {
                    if (this.IsCutom())
                    {
                        this.Preset = AQPresets.Custom;
                    }
                }
            }
        }

        public AQTK_VOICE ToParameter() =>
            new AQTK_VOICE()
            {
                bas = (int)this.BaseVoice,
                spd = this.Speed,
                vol = this.Volume,
                pit = this.Pitch,
                acc = this.Accent,
                lmd = this.LMD,
                fsc = this.FSC,
            };

        public override string ToString() =>
            $"Preset:{this.Preset}," +
            $"BaseVoice:{this.BaseVoice}," +
            $"Speed:{this.Speed}," +
            $"Volume:{this.Volume}," +
            $"Pitch:{this.Pitch}," +
            $"Accent:{this.Accent}," +
            $"LMD:{this.LMD}" +
            $"FSC:{this.FSC}";
    }
}
