namespace AudioMux.Tracks;

public class SignalSequencer<T> : ITrack<T>
    where T : unmanaged
{
    private readonly ITrack<T> track;
    private readonly int signal;
    private readonly int[][] sequences;
    private int sequenceIndex = 0;

    public SignalSequencer(ITrack<T> track, int signal, int[][] sequences)
    {
        this.track = track;
        this.signal = signal;
        this.sequences = sequences;
    }

    public void Signal(int signal)
    {
        if (signal == this.signal)
        {
            if (sequenceIndex > sequences.Length)
                sequenceIndex = 0;
            int[] sequence = sequences[sequenceIndex++];
            foreach (int seq in sequence)
                track.Signal(seq);
        }
        else
        {
            track.Signal(signal);
        }
    }

    public int SampleRateHz => track.SampleRateHz;

    public TimeSpan Length => track.Length;

    public TimeSpan CurrentPosition => track.CurrentPosition;

    public int Channels => track.Channels;

    public bool AdvanceMusicData()
    {
        return track.AdvanceMusicData();
    }

    public void Dispose()
    {
        track.Dispose();
    }

    public MusicData<T> GetCurrentMusicData()
    {
        return track.GetCurrentMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        track.SeekMusicData(span);
    }
}
