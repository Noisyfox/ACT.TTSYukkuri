using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ACT.TTSYukkuri.Config;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.VoiceNext;
using DSharpPlus.VoiceNext.Codec;
using FFXIV.Framework.Bridge;
using FFXIV.Framework.Common;
using NLog;
using Prism.Mvvm;

namespace ACT.TTSYukkuri.Discord.Models
{
    public class DiscordClientModel :
        BindableBase
    {
        #region Singleton

        private static DiscordClientModel instance = new DiscordClientModel();

        public static DiscordClientModel Instance => instance;

        #endregion Singleton

        #region Logger

        private Logger Logger => AppLog.DefaultLogger;

        #endregion Logger

        private bool connected;

        public bool Connected
        {
            get => this.connected;
            set => this.SetProperty(ref this.connected, value);
        }

        private bool joinedVoiceChannel;

        public bool JoinedVoiceChannel
        {
            get => this.joinedVoiceChannel;
            set => this.SetProperty(ref this.joinedVoiceChannel, value);
        }

        public const string DicordCommandPrefix = "//";

        private DiscordSettings Config => Settings.Default.DiscordSettings;

        public void Initialize()
        {
            // Bridgeにデリゲートを登録する
            DiscordBridge.Instance.SendMessageDelegate = this.SendMessage;
            DiscordBridge.Instance.SendSpeakingDelegate = this.Play;
        }

        public void Dispose()
        {
            // Bridgeのデリゲートを解除する
            DiscordBridge.Instance.SendMessageDelegate = null;
            DiscordBridge.Instance.SendSpeakingDelegate = null;

            this.Disconnect();
        }

        private DiscordChannel selectedTextChannel;

        public DiscordChannel SelectedTextChannel
        {
            get => this.selectedTextChannel;
            set
            {
                this.selectedTextChannel = value;
                this.Config.DefaultTextChannelID = value?.Id ?? 0;
            }
        }

        private DiscordChannel selectedVoiceChannel;

        public DiscordChannel SelectedVoiceChannel
        {
            get => this.selectedVoiceChannel;
            set
            {
                this.selectedVoiceChannel = value;
                this.Config.DefaultVoiceChannelID = value?.Id ?? 0;
            }
        }

        public string GuildName => string.Join(
            Environment.NewLine,
            this.guilds.OrderBy(x => x.Id).Select(x => x.Name).ToArray());

        public IReadOnlyList<DiscordChannel> Channels
        {
            get
            {
                var list = new List<DiscordChannel>();
                foreach (var guild in this.guilds.OrderBy(x => x.Id))
                {
                    list.AddRange(guild.Channels);
                }

                return (
                    from x in list
                    orderby
                    x.Type,
                    x.Id
                    select
                    x).ToList();
            }
        }

        public IReadOnlyList<DiscordChannel> TextChannels =>
            this.Channels?.Where(x => x.Type == ChannelType.Text).ToList();

        public IReadOnlyList<DiscordChannel> VoiceChannels =>
            this.Channels?.Where(x => x.Type == ChannelType.Voice).ToList();

        public string Log => this.log.ToString();

        private StringBuilder log = new StringBuilder();
        private DiscordClient discord;
        private readonly List<DiscordGuild> guilds = new List<DiscordGuild>();
        private VoiceNextClient voice;
        private VoiceNextConnection vnc;

        private bool isInit = false;

        public async void Connect(
            bool isInit = false)
        {
            this.isInit = isInit;

            this.Disconnect();

            if (string.IsNullOrEmpty(this.Config.Token))
            {
                return;
            }

            this.discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = this.Config.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = DSharpPlus.LogLevel.Error,
                UseInternalLogHandler = true
            });

            this.discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().Contains($"{DicordCommandPrefix}ping"))
                {
                    await e.Message.RespondAsync("pong");
                }
            };

            this.discord.Ready += this.Ready;
            this.discord.GuildAvailable += this.GuildAvailable;
            this.discord.ClientErrored += this.ClientError;

            var vcfg = new VoiceNextConfiguration
            {
                VoiceApplication = VoiceApplication.Voice
            };

            this.voice = this.discord.UseVoiceNext(vcfg);

            try
            {
                await this.discord.ConnectAsync();
            }
            catch (Exception ex)
            {
                this.AppendLogLine("Connection failed.", ex);
            }
        }

        public async void Disconnect()
        {
            if (this.discord != null)
            {
                await this.discord?.DisconnectAsync()
                    .ContinueWith((task) =>
                {
                    if (this.vnc != null)
                    {
                        this.vnc.Dispose();
                        this.vnc = null;
                        this.voice = null;
                    }

                    this.guilds.Clear();
                    this.discord = null;

                    this.Connected = false;
                    this.JoinedVoiceChannel = false;

                    this.AppendLogLine("Disconnected from Guild.");

                    this.RaisePropertyChanged(nameof(this.GuildName));
                    this.RaisePropertyChanged(nameof(this.Channels));
                    this.RaisePropertyChanged(nameof(this.TextChannels));
                    this.RaisePropertyChanged(nameof(this.VoiceChannels));
                });
            }
        }

        public async void JoinVoiceNext()
        {
            var chn = this.SelectedVoiceChannel;
            if (chn == null)
            {
                return;
            }

            // libopus.dll
            // libsodium.dll
            // の存在を確認する
            var entryDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var opus = Path.Combine(entryDirectory, "libopus.dll");
            var sodium = Path.Combine(entryDirectory, "libsodium.dll");
            if (!File.Exists(opus))
            {
                this.AppendLogLine($"Join Error", new FileNotFoundException("Opus not found.", opus));
                return;
            }

            if (!File.Exists(sodium))
            {
                this.AppendLogLine($"Join Error", new FileNotFoundException("Sodium not found.", sodium));
                return;
            }

            this.vnc = await this.voice.ConnectAsync(chn)
                .ContinueWith<VoiceNextConnection>((task) =>
                {
                    this.AppendLogLine($"Joined channel: {chn.Name}");

                    this.isInit = false;
                    this.JoinedVoiceChannel = true;

                    return task.Result;
                });
        }

        public async void Leave()
        {
            if (this.discord != null)
            {
                await this.discord.DisconnectAsync()
                    .ContinueWith(async (task) =>
                    {
                        if (this.vnc != null)
                        {
                            this.vnc.Dispose();
                            this.vnc = null;
                            this.voice = null;
                        }

                        this.guilds.Clear();
                        this.discord = null;

                        this.Connected = false;
                        this.JoinedVoiceChannel = false;

                        this.AppendLogLine($"Left channel.");

                        this.RaisePropertyChanged(nameof(this.GuildName));
                        this.RaisePropertyChanged(nameof(this.Channels));
                        this.RaisePropertyChanged(nameof(this.TextChannels));
                        this.RaisePropertyChanged(nameof(this.VoiceChannels));

                        await Task.Delay(TimeSpan.FromMilliseconds(200));
                        this.Connect();
                    });
            }
        }

        private DateTime lastSpeakTimestamp;

        public void SendMessage(
            string message,
            bool tts = false)
        {
            try
            {
                var chn = this.SelectedTextChannel;
                if (chn != null)
                {
                    chn.SendMessageAsync(message, tts);
                }
            }
            catch (Exception ex)
            {
                this.AppendLogLine($"Send Message error !", ex);
            }
        }

        public async void Play(
            string wave)
        {
            try
            {
                lock (this)
                {
                    if ((DateTime.Now - this.lastSpeakTimestamp).TotalSeconds
                        <= Settings.Default.DiscordSettings.SpeakInterval)
                    {
                        return;
                    }

                    this.lastSpeakTimestamp = DateTime.Now;
                }

                if (this.vnc != null)
                {
                    try
                    {
                        await vnc.SendSpeakingAsync(true);

                        WaveModel.Instance.SendEncode(
                            wave,
                            (bytes, blocksize) => this.vnc.SendAsync(bytes, blocksize));
                    }
                    finally
                    {
                        await vnc.SendSpeakingAsync(false);
                    }

                    this.AppendLogLine($"Play sound: {wave}");
                }
            }
            catch (Exception ex)
            {
                this.AppendLogLine($"Play sound error !", ex);
            }
        }

        private Task ClientError(
            ClientErrorEventArgs e)
        {
            this.AppendLogLine(
                $"Client error. event: {e.EventName}",
                e.Exception);

            this.Connected = false;
            this.JoinedVoiceChannel = false;

            return Task.CompletedTask;
        }

        private Task Ready(
            ReadyEventArgs e)
        {
            this.AppendLogLine("Client is Ready.");
            return Task.CompletedTask;
        }

        private Task GuildAvailable(
            GuildCreateEventArgs e)
        {
            this.guilds.Add(e.Guild);

            if (e.Guild.Channels.Any())
            {
                var ch = default(DiscordChannel);

                ch = this.TextChannels.FirstOrDefault(x => x.Id == this.Config.DefaultTextChannelID);
                if (ch != null)
                {
                    this.SelectedTextChannel = ch;
                }

                ch = this.VoiceChannels.FirstOrDefault(x => x.Id == this.Config.DefaultVoiceChannelID);
                if (ch != null)
                {
                    this.SelectedVoiceChannel = ch;
                }
            }

            this.RaisePropertyChanged(nameof(this.GuildName));
            this.RaisePropertyChanged(nameof(this.Channels));
            this.RaisePropertyChanged(nameof(this.TextChannels));
            this.RaisePropertyChanged(nameof(this.VoiceChannels));
            this.RaisePropertyChanged(nameof(this.SelectedTextChannel));
            this.RaisePropertyChanged(nameof(this.SelectedVoiceChannel));

            this.AppendLogLine($"Guild available: {e.Guild.Name}");

            this.Connected = true;

            if (this.isInit)
            {
                if (this.Config.AutoJoin &&
                    this.Config.DefaultVoiceChannelID != 0 &&
                    this.SelectedVoiceChannel != null)
                {
                    this.isInit = false;
                    this.JoinVoiceNext();
                }
            }

            return Task.CompletedTask;
        }

        private void AppendLogLine(
            string message,
            Exception ex = null)
        {
            // NLogに出力する
            if (ex == null)
            {
                this.Logger.Trace(message);
            }
            else
            {
                this.Logger.Error(ex, message);
            }

            // UIに出力する
            var text = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}] {message}";
            if (ex != null)
            {
                text += Environment.NewLine + ex.ToString();
            }

            this.log.AppendLine(text);
            this.RaisePropertyChanged(nameof(this.Log));

            var log = $"[DISCORD] {message}";
            if (ex == null)
            {
                this.Logger.Info(log);
            }
            else
            {
                this.Logger.Error(ex, log);
            }
        }
    }
}
