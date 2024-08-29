using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lore.Music;

public class MusicModel
{
    private readonly MusicContext ctx;
    private readonly ILogger<MusicModel> logger;

    public MusicModel(MusicContext ctx, ILogger<MusicModel> logger)
    {
        this.ctx = ctx;
        this.logger = logger;
    }

    public async Task<bool> Register(ulong serverId, string name, Stream track, MusicType type)
    {
        if (type == MusicType.Unknown)
            throw new InvalidOperationException();
        bool any = await ctx.Music.AnyAsync(m => m.ServerId == serverId && m.Name == name);
        if (any) return false;
        using MemoryStream ms = new MemoryStream();
        await track.CopyToAsync(ms);
        MusicEntryEntity entity = new()
        {
            ServerId = serverId,
            Name = name,
            Stream = ms.ToArray(),
            Type = type
        };
        ctx.Add(entity);
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<List<Track>> List(ulong serverId)
    {
        return await ctx.Music
            .Where(m => m.ServerId == serverId)
            .Select(m => new Track(m.Name, m.Type))
            .ToListAsync();
    }

    public async Task<bool> Delete(ulong serverId, string name)
    {
        MusicEntryEntity? entity = await ctx.Music
            .Where(m => m.ServerId == serverId && m.Name == name)
            .SingleOrDefaultAsync();
        if (entity is null) return false;
        ctx.Remove(entity);
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<MusicTrackData?> Get(ulong serverId, string name)
    {
        MusicEntryEntity? entity = await ctx.Music
            .Where(m => m.ServerId == serverId && m.Name == name)
            .SingleOrDefaultAsync();
        if (entity is null) return null;
        return new MusicTrackData(entity.Stream, entity.Type);
    }
}

public class MusicTrackData
{
    public readonly byte[] Array;
    public readonly MusicType Type;

    public int Length => Array.Length;

    public unsafe MusicTrackData(byte[] array, MusicType Type)
    {
        Array = array;
        this.Type = Type;
    }
}
