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

        private bool joined;

        public bool Joined
        {
            get => this.joined;
            set => this.SetProperty(ref this.joined, value);
        }

        public const string DicordCommandPrefix = "//";

        private DiscordSettings Config => Settings.Default.DiscordSettings;

        public void Initialize()
        {
        }

        public void Dispose()
        {
            this.Disconnect();
        }

        private DiscordChannel selectedChannel;

        public DiscordChannel SelectedChannel
        {
            get => this.selectedChannel;
            set
            {
                this.selectedChannel = value;
                this.Config.DefaultChannelID = this.SelectedChannel.Id;
            }
        }

        public string GuildName => this.guild?.Name;

        public IReadOnlyList<DiscordChannel> Channels => this.guild?.Channels
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Id)
            .ToList();

        public string Log => this.log.ToString();

        private StringBuilder log = new StringBuilder();
        private DiscordClient discord;
        private DiscordGuild guild;
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

            await this.discord.ConnectAsync();
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
                    }

                    this.discord = null;

                    this.Connected = false;
                    this.Joined = false;
                });
            }
        }

        public async void Join()
        {
            var chn = this.SelectedChannel;
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

                    this.Joined = true;

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
                        }

                        this.discord = null;

                        this.Joined = false;

                        this.AppendLogLine($"Left channel.");

                        await Task.Delay(TimeSpan.FromMilliseconds(200));
                        this.Connect();
                    });
            }
        }

        private DateTime lastSpeakTimestamp;

        public async void Play(
            string wave)
        {
            try
            {
                lock (this)
                {
                    if ((DateTime.Now - this.lastSpeakTimestamp).TotalSeconds <= 0.2d)
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
            this.Joined = false;

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
            this.guild = e.Guild;

            if (e.Guild.Channels.Any())
            {
                if (this.Config.DefaultChannelID == 0)
                {
                    this.SelectedChannel = (
                        from x in e.Guild.Channels
                        orderby
                        x.Type descending,
                        x.Id
                        select
                        x).FirstOrDefault();
                }
                else
                {
                    this.SelectedChannel = e.Guild.Channels
                        .FirstOrDefault(x =>
                            x.Id == this.Config.DefaultChannelID);
                }
            }

            this.RaisePropertyChanged(nameof(this.GuildName));
            this.RaisePropertyChanged(nameof(this.Channels));
            this.RaisePropertyChanged(nameof(this.SelectedChannel));

            this.AppendLogLine($"Guild available: {e.Guild.Name}");

            this.Connected = true;

            if (this.isInit)
            {
                this.isInit = false;

                if (this.Config.AutoJoin &&
                    this.Config.DefaultChannelID != 0 &&
                    this.SelectedChannel != null)
                {
                    this.Join();
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
                text += ex.ToString();
            }

            this.log.AppendLine(text);
            this.RaisePropertyChanged(nameof(this.Log));
        }
    }
}
