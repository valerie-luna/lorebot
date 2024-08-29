using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using AudioMux.Tracks;
using Lore.Discord;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.Logging;
using Lore.Utilities;

namespace Lore.Music;

public class MusicCommand : ICommand
{
    private const DiscordPermissions Permission = DiscordPermissions.Administrator;
    private readonly ILogger<MusicCommand> logger;
    private readonly MusicModel music;
    private readonly IHttpClientFactory clientFactory;
    private readonly MusicThreadHandler handler;

    public MusicCommand(ILogger<MusicCommand> logger, MusicModel music,
        IHttpClientFactory clientFactory, MusicThreadHandler handler)
    {
        this.logger = logger;
        this.music = music;
        this.clientFactory = clientFactory;
        this.handler = handler;
    }

    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        builder.WithName("music")
            .WithDescription("Play music.");

        var play = new DiscordApplicationCommandOptionBuilder()
            .WithName("play")
            .WithDescription("Play music")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "track",
                type: DiscordApplicationCommandOptionType.String,
                description: "The track to play.",
                isRequired: true
            ).AddOption(
                name: "channel",
                type: DiscordApplicationCommandOptionType.Channel,
                description: "The channel to play in, if not your channel.",
                isRequired: false
            );

        var stop = new DiscordApplicationCommandOptionBuilder()
            .WithName("stop")
            .WithDescription("Stop playing music.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand);

        var upload = new DiscordApplicationCommandOptionBuilder()
            .WithName("upload")
            .WithDescription("Upload a track")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "name",
                type: DiscordApplicationCommandOptionType.String,
                description: "The name of the track.",
                isRequired: true
            ).AddOption(
                name: "track",
                type: DiscordApplicationCommandOptionType.Attachment,
                description: "The track to upload.",
                isRequired: true
            );

        var list = new DiscordApplicationCommandOptionBuilder()
            .WithName("list")
            .WithDescription("List tracks in this server.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand);

        var delete =  new DiscordApplicationCommandOptionBuilder()
            .WithName("delete")
            .WithDescription("Remove a track.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "track",
                type: DiscordApplicationCommandOptionType.String,
                description: "The track to remove.",
                isRequired: true
            );

        builder.AddOption(play)
            .AddOption(upload)
            .AddOption(list)
            .AddOption(delete)
            .AddOption(stop);

        return "music";
    }

    public async Task Execute(DiscordInteraction command)
    {
        await CheckPermission(command, Permission);
        var option = command.Data.Options.Single();
        var task = option.Name switch
        {
            "play" => Play(command),
            "stop" => Stop(command),
            "upload" => Upload(command),
            "list" => List(command),
            "delete" => Delete(command),
            _ => throw new NotImplementedException()
        };
        await task;
    }

    private async Task Play(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var channelId = subcommand.GetOptionValueOrDefault<ulong?>("channel");
        var name = subcommand.GetOptionValue<string>("track");

        DiscordChannel? channel;
        if (channelId is not null)
        {
            channel = command.Data.Resolved.Channels[channelId.Value];
        }
        else
        {
            channel = (command.User as DiscordMember)?.VoiceState?.Channel;
        }
        if (channel is null)
        {
            await command.RespondAsync("Could not resolve music channel - are you not in voice?", ephemeral: true);
            return;
        }
        if (channel.Type != DiscordChannelType.Voice)
        {
            await command.RespondAsync("Can't join voice in a non-voice channel.", ephemeral: true);
            return;
        }

        MusicTrackData? data = await music.Get(command.Guild.Id, name);

        if (data is null)
        {
            await command.RespondAsync($"Can't find track '{name}'", ephemeral: true);
            return;
        }

        var client = await channel.ConnectAsync();
        
        handler.Start(client, data);

        await command.RespondAsync("Begun music!", ephemeral: true);
    }

    private async Task Stop(DiscordInteraction command)
    {
        bool success = handler.Stop(command.Guild.Id);

        if (success)
        {
            await command.RespondAsync("Stopped music.", ephemeral: true);
        }
        else
        {
            await command.RespondAsync("Music was not active.");
        }
    }

    private async Task Upload(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("name").ToLowerInvariant().Trim();
        var attachmentData = command.Data.Resolved.Attachments.Single().Value;

        MusicType type = attachmentData.MediaType switch
        {
            "audio/wav" => MusicType.Wav,
            "audio/flac" => MusicType.Flac,
            "audio/mpeg" => MusicType.Mp3,
            _ => MusicType.Unknown
        };

        if (type == MusicType.Unknown)
        {
            await command.RespondAsync($"Unknown music type (read {attachmentData.MediaType})", ephemeral: true);
            return;
        }

        var client = clientFactory.CreateClient();
        var attachment = await client.GetStreamAsync(attachmentData.ProxyUrl);

        await music.Register(command.Guild.Id, name, attachment, type);

        await command.RespondAsync($"Registered track as '{name}'", ephemeral: true);
    }

    private async Task List(DiscordInteraction command)
    {
        var tracks = await music.List(command.Guild.Id);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("**Registered tracks:**");
        int i = 1;
        foreach (var track in tracks)
            builder.AppendLine($"**{i++}**: {track.Name}");
        if (tracks.Count == 0)
            builder.AppendLine("No tracks registered.");

        await command.RespondAsync(builder.ToString(), ephemeral: true);
    }

    private async Task Delete(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("track").ToLowerInvariant().Trim();

        var success = await music.Delete(command.Guild.Id, name);
        if (success)
            await command.RespondAsync($"Removed '{name}'", ephemeral: true);
        else
            await command.RespondAsync($"Could not find '{name}'", ephemeral: true);
    }

    private static async Task CheckPermission(DiscordInteraction command, DiscordPermissions permission)
    {
        var user = command.User as DiscordMember;
        Debug.Assert(user is not null);
        if ((user.Permissions & permission) == 0)
        {
            await command.RespondAsync("You are not permitted to use this command.", ephemeral: true);
            throw new CommandFinishedException();
        }
    }
}

public class MusicThreadHandler
{
    private readonly ConcurrentDictionary<ulong, CancellationTokenSource?> threads = new();
    
    public void Start(VoiceNextConnection connection, MusicTrackData data)
    {
        var id = connection.TargetChannel.Guild.Id;
        var cts = new CancellationTokenSource();
        connection.UserLeft += (conn, args) =>
        {
            if (conn.TargetChannel.Users.Count == 1)
                cts.CancelAfter(TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        };
        connection.UserJoined += (conn, args) =>
        {
            cts.CancelAfter(Timeout.Infinite);
            return Task.CompletedTask;
        };
        Task.Run(() => RunTrack(connection, data, cts.Token, () => CancelThread(id)));
        threads[id] = cts;
    }

    private record MusicThreadState();

    private const int MsSampleSize = 200;
    private async static void RunTrack(VoiceNextConnection connection, 
        MusicTrackData data, CancellationToken ct, Action onDone)
    {
        GCHandle handle = GCHandle.Alloc(data.Array, GCHandleType.Pinned);
        try
        {
            ITrack<float> basetrack;
            unsafe
            {
                byte* ptr = (byte*)handle.AddrOfPinnedObject();
                basetrack = data.Type switch
                {
                    MusicType.Flac => new FlacTrack(ptr, data.Length),
                    MusicType.Mp3 => new Mp3Track(ptr, data.Length),
                    MusicType.Wav => new WavTrack(ptr, data.Length),
                    _ => throw new NotImplementedException(),
                };
            }
            basetrack = new Repeat<float>(basetrack);
            ITrack<short> track = new SoxrSampleAndReformat(basetrack, 48000);
            track = new ConstBuffer<short>(track, TimeSpan.FromMilliseconds(MsSampleSize));
            var sink = connection.GetTransmitSink();
            byte[]? buffer = null;
            while (!ct.IsCancellationRequested)
            {
                var success = track.AdvanceMusicData();
                if (!success)
                    break;
                RefStructNonsense(track, ref buffer);
                await sink.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        finally
        {
            handle.Free();
            connection.Disconnect();
            onDone();
        }

        static unsafe void RefStructNonsense(ITrack<short> track, [NotNull] ref byte[]? buffer)
        {
            var data = track.GetCurrentMusicData();
            buffer ??= new byte[data.Length * 2 * sizeof(short)];
            Debug.Assert(buffer.Length == data.Length * 2 * sizeof(short));
            fixed (short* ptr = data.AsSpan())
            {
                Marshal.Copy((nint)ptr, buffer, 0, buffer.Length);
            }
        }
    }

    public bool Stop(ulong guildId)
    {
        if (threads.TryGetValue(guildId, out CancellationTokenSource? t) && t is not null)
        {
            t.Cancel();
            CancelThread(guildId);
            return true;
        }
        return false;
    }

    private void CancelThread(ulong id)
    {
        var cts = threads[id];
        threads[id] = null;
        cts?.Dispose();
    }
}