using System.Numerics;
using System.Runtime.CompilerServices;

namespace AudioMux.Tracks;

public sealed class Mix : ITrack<float>
{
    private readonly ConstBuffer<float>[] tracks;
    private int CurrentPositionSamples = -1;
    private bool disposedValue;
    private readonly MusicDataInternal<float> musicData;
    private readonly MixStrategy strategy;

    public unsafe Mix(TimeSpan chunksize, MixStrategy strategy, params ITrack<float>[] tracks)
    {
        if (tracks.Length is 0 or 1)
        {
            throw new InvalidOperationException("Need to mix at least two tracks.");
        }
        int index = 0;
        this.tracks = new ConstBuffer<float>[tracks.Length];
        foreach (var track in tracks)
        {
            if (track is ConstBuffer<float> constbuffer)
            {
                if (constbuffer.Chunksize != chunksize)
                    throw new InvalidOperationException("Invalid sized buffer");
                this.tracks[index++] = constbuffer;
            }
            else
            {
                this.tracks[index++] = new ConstBuffer<float>(track, chunksize);
            }
        }
        if (tracks.Select(b => b.SampleRateHz).Distinct().Count() != 1)
            throw new InvalidOperationException("Cannot mix sample rates");
        if (tracks.Select(b => b.Channels).Distinct().Count() != 1)
            throw new InvalidOperationException("Cannot mix channel counts");
        var elements = (int)(chunksize.TotalSeconds * tracks[0].SampleRateHz);
        musicData = new MusicDataInternal<float>(
            elements,
            tracks[0].Channels
        );
        this.strategy = strategy;
    }

    public int SampleRateHz => tracks[0].SampleRateHz;

    public TimeSpan Length => tracks.Select(b => b.Length).Max();

    public TimeSpan CurrentPosition => TimeSpan.FromSeconds(CurrentPositionSamples * (double)SampleRateHz);

    public int Channels => tracks[0].Channels;

    public unsafe bool AdvanceMusicData()
    {
        if (CurrentPositionSamples == -1)
            CurrentPositionSamples = 0;
        else
            CurrentPositionSamples += musicData.Length;
        MusicData<float> data;
        int dataCount = 0;
        int length = musicData.Length * Channels;
        int remaining = length % Vector<float>.Count;
        var musicspan = musicData.AsSpan();
        musicspan.Clear();
        foreach (var track in tracks)
        {
            if (!track.AdvanceMusicData()) continue;
            data = track.GetCurrentMusicData();
            for (int d = 0; d < dataCount; d++)
            {
                var dataspan = data.AsSpan();
                for (int i = 0; i < (length - remaining); i += Vector<float>.Count)
                {
                    var v1 = new Vector<float>(musicspan[i..]);
                    var v2 = new Vector<float>(dataspan[i..]);
                    (v1 + v2).CopyTo(musicspan[i..]);
                }
                for (int i = length - remaining; i < length; i++)
                {
                    musicspan[i] += dataspan[i];
                }
            }
        }
        for (int i = 0; i < length; i++)
        {
            switch (strategy)
            {
                case MixStrategy.Nothing:
                    break;
                case MixStrategy.Divide:
                    musicspan[i] /= dataCount;
                    break;
                default: throw new NotImplementedException();
            }
        }
        return true;
    }

    public MusicData<float> GetCurrentMusicData()
    {
        return musicData.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        foreach (var track in tracks)
            track.SeekMusicData(span);
    }

    public void Signal(int signal)
    {
        foreach (var track in tracks)
            track.Signal(signal);
    }

    private unsafe void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var track in tracks)
                    track.Dispose();
            }

            musicData.Unallocate();

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Mix()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
