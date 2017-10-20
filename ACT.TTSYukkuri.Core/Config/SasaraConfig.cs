using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FFXIV.Framework.TTS.Common;
using FFXIV.Framework.TTS.Common.Models;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Config
{
    /// <summary>
    /// TTSささら設定
    /// </summary>
    [Serializable]
    public class SasaraConfig :
        BindableBase
    {
        private CevioTalkerModel talker;

        private string cast;
        private uint onryo;
        private uint hayasa;
        private uint takasa;
        private uint seishitsu;
        private uint yokuyo;
        private ObservableCollection<SasaraComponent> components = new ObservableCollection<SasaraComponent>();

        public SasaraConfig()
        {
            this.components.CollectionChanged += this.ComponentsCollectionChanged;
        }

        /// <summary>
        /// 有効なキャストのリスト
        /// </summary>
        public ObservableCollection<string> AvalableCasts
        {
            get;
            private set;
        } = new ObservableCollection<string>();

        /// <summary>
        /// キャスト
        /// </summary>
        public string Cast
        {
            get => this.cast;
            set
            {
                if (this.SetProperty(ref this.cast, value))
                {
                    this.SetCast(this.cast);
                }
            }
        }

        /// <summary>
        /// 感情コンポーネント
        /// </summary>
        public ObservableCollection<SasaraComponent> Components
        {
            get => this.components;
            set
            {
                var previousComponents = this.components;

                if (this.SetProperty(ref this.components, value))
                {
                    previousComponents.CollectionChanged -= this.ComponentsCollectionChanged;
                    this.components.CollectionChanged += this.ComponentsCollectionChanged;
                }
            }
        }

        private void ComponentsCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e) => this.SyncRemoteModel();

        /// <summary>
        /// 音量
        /// </summary>
        public uint Onryo
        {
            get => this.onryo;
            set
            {
                if (this.SetProperty(ref this.onryo, value))
                {
                    this.SyncRemoteModel();
                }
            }
        }

        /// <summary>
        /// 早さ
        /// </summary>
        public uint Hayasa
        {
            get => this.hayasa;
            set
            {
                if (this.SetProperty(ref this.hayasa, value))
                {
                    this.SyncRemoteModel();
                }
            }
        }

        /// <summary>
        /// 高さ
        /// </summary>
        public uint Takasa
        {
            get => this.takasa;
            set
            {
                if (this.SetProperty(ref this.takasa, value))
                {
                    this.SyncRemoteModel();
                }
            }
        }

        /// <summary>
        /// 声質
        /// </summary>
        public uint Seishitsu
        {
            get => this.seishitsu;
            set
            {
                if (this.SetProperty(ref this.seishitsu, value))
                {
                    this.SyncRemoteModel();
                }
            }
        }

        /// <summary>
        /// 抑揚
        /// </summary>
        public uint Yokuyo
        {
            get => this.yokuyo;
            set
            {
                if (this.SetProperty(ref this.yokuyo, value))
                {
                    this.SyncRemoteModel();
                }
            }
        }

        /// <summary>
        /// 内容をMD5化する
        /// </summary>
        /// <returns></returns>
        public string GetMD5()
        {
            var sb = new StringBuilder();
            sb.AppendLine(this.Cast);
            sb.AppendLine(this.Onryo.ToString());
            sb.AppendLine(this.Hayasa.ToString());
            sb.AppendLine(this.Takasa.ToString());
            sb.AppendLine(this.Seishitsu.ToString());
            sb.AppendLine(this.Yokuyo.ToString());

            if (this.Components != null)
            {
                foreach (var c in this.Components)
                {
                    sb.AppendLine(c.Id + c.Name + c.Value.ToString());
                }
            }

            return sb.ToString().GetMD5();
        }

        /// <summary>
        /// リモートに自動的に反映するか？
        /// </summary>
        [XmlIgnore]
        public bool AutoSync { get; set; } = true;

        /// <summary>
        /// リモートに反映する
        /// </summary>
        private void SyncRemoteModel()
        {
            if (this.AutoSync)
            {
                this.SetToRemote();
            }
        }

        /// <summary>
        /// リモートの設定を読み込む
        /// </summary>
        public void LoadRemoteConfig()
        {
            this.talker = RemoteTTSClient.Instance.TTSModel?.GetCevioTalker();
            if (this.talker == null)
            {
                return;
            }

            // 有効なキャストを列挙する
            var addCasts = this.talker.AvailableCasts
                .Where(x => !this.AvalableCasts.Contains(x));
            var removeCasts = this.AvalableCasts
                .Where(x => !this.talker.AvailableCasts.Contains(x))
                .ToArray();

            this.AvalableCasts.AddRange(addCasts);
            foreach (var item in removeCasts)
            {
                this.AvalableCasts.Remove(item);
            }

            if (string.IsNullOrWhiteSpace(this.Cast))
            {
                // キャストを設定する
                var firstCast = this.talker.AvailableCasts.FirstOrDefault();
                this.SetCast(firstCast);
            }
            else
            {
                // 現在の設定をリモートに送る
                this.SetToRemote();
            }
        }

        /// <summary>
        /// キャストを変更する
        /// </summary>
        /// <param name="cast">
        /// キャスト</param>
        public void SetCast(
            string cast)
        {
            if (string.IsNullOrWhiteSpace(cast) ||
               this.talker == null ||
               this.talker.Cast == cast)
            {
                return;
            }

            try
            {
                this.AutoSync = false;

                this.talker.Cast = cast;
                RemoteTTSClient.Instance.TTSModel.SetCevioTalker(this.talker);
                this.talker = RemoteTTSClient.Instance.TTSModel.GetCevioTalker();

                this.Components.Clear();
                for (int i = 0; i < talker.Components.Count; i++)
                {
                    var component = talker.Components[i];
                    this.Components.Add(new SasaraComponent()
                    {
                        Id = component.Id,
                        Name = component.Name.Trim(),
                        Value = component.Value,
                        Cast = talker.Cast,
                    });
                }

                this.Cast = cast;
                this.Onryo = talker.Volume;
                this.Hayasa = talker.Speed;
                this.Takasa = talker.Tone;
                this.Seishitsu = talker.Alpha;
                this.Yokuyo = talker.ToneScale;
            }
            finally
            {
                this.AutoSync = true;
            }
        }

        /// <summary>
        /// ささらの設定を取得する
        /// </summary>
        /// <returns>
        /// ささらの設定モデル</returns>
        public CevioTalkerModel ToRemoteModel()
        {
            if (this.talker == null)
            {
                this.talker = new CevioTalkerModel();
            }

            this.talker.Cast = this.Cast;
            this.talker.Volume = this.Onryo;
            this.talker.Speed = this.Hayasa;
            this.talker.Tone = this.Takasa;
            this.talker.Alpha = this.Seishitsu;
            this.talker.ToneScale = this.Yokuyo;

            var components = new List<CevioTalkerModel.CevioTalkerComponent>();
            foreach (var c in this.Components)
            {
                components.Add(new CevioTalkerModel.CevioTalkerComponent()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Value = c.Value
                });
            }

            this.talker.Components = components;

            return this.talker;
        }

        /// <summary>
        /// ささらを設定する
        /// </summary>
        public void SetToRemote(CevioTalkerModel remoteModel) =>
            RemoteTTSClient.Instance.TTSModel?.SetCevioTalker(remoteModel);

        /// <summary>
        /// ささらを設定する
        /// </summary>
        public void SetToRemote() =>
            this.SetToRemote(this.ToRemoteModel());
    }
}
