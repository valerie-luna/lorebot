using AudioMux.Crossfade;

namespace AudioMux.Tracks;

public sealed class TrackSwitcher<T, TMix> : ITrack<T>
    where T : unmanaged
    where TMix : ICrossfade<T>
{
    private readonly ITrack<T> left;
    private readonly ITrack<T> right;
    private readonly int signal;
    private readonly TimeSpan mixTime;
    private bool switchedToRight;
    private ITrackData<T>? switching;

    public TrackSwitcher(ITrack<T> left, ITrack<T> right, int signal, TimeSpan mixTime)
    {
        if (left.SampleRateHz != right.SampleRateHz)
            throw new InvalidOperationException();
        if (left.Channels != right.Channels)
            throw new InvalidOperationException();
        this.left = left;
        this.right = right;
        this.signal = signal;
        this.mixTime = mixTime;
    }

    public int SampleRateHz => left.SampleRateHz;

    public TimeSpan Length => switchedToRight ? right.Length : left.Length;

    public int Channels => left.Channels;

    public TimeSpan CurrentPosition => switchedToRight ? right.CurrentPosition : left.CurrentPosition;

    public bool AdvanceMusicData()
    {
        if (switching is not null)
        {
            bool result = switching.AdvanceMusicData();
            if (result)
                return result;
            else
            {
                switching.Dispose();
                switching = null;
                return true;
            }
        }
        return switchedToRight
            ? right.AdvanceMusicData()
            : left.AdvanceMusicData();
}

    public void Dispose()
    {
        left.Dispose();
        right.Dispose();
    }

    public void Signal(int signal)
    {
        if (signal == this.signal)
        {
            switchedToRight = !switchedToRight;
            if (switchedToRight)
            {
                switching = TMix.Mix(left, right, mixTime);
            }
            else
            {
                switching = TMix.Mix(right, left, mixTime);
            }
        }
        else if (switchedToRight)
        {
            right.Signal(signal);
        }
        else
        {
            left.Signal(signal);
        }
    }

    public MusicData<T> GetCurrentMusicData()
    {
        if (switching is not null)
        {
            return switching.GetCurrentMusicData();
        }
        else
        {
            if (switchedToRight)
            {
                return right.GetCurrentMusicData();
            }
            else
            {
                return left.GetCurrentMusicData();
            }
        }
    }

    // this seems like a bad idea. maybe just 
    public void SeekMusicData(TimeSpan span)
    {
        if (switchedToRight)
        {
            right.SeekMusicData(span);
        }
        else
        {
            left.SeekMusicData(span);
        }
    }
}