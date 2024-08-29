using System.Runtime.CompilerServices;
using AudioMux.Libraries.DrWav;

namespace AudioMux.Tracks;

public unsafe class WavTrack : ITrack<float>
{
    private DrWav data;
    private readonly ulong frameCount;
    private MusicDataInternal<float> musicdata;
    private ulong currentFrame;
    private bool disposedValue;
    public WavTrack(string filename)
    {
        bool success = DrWavLibrary.InitFile(ref data, filename, ref Unsafe.NullRef<AllocationCallbacks>());
        if (!success) throw new InvalidOperationException();
        var result = DrWavLibrary.GetLengthPcmFrames(ref data, out frameCount);
        if (result != DrwavResult.Success) throw new InvalidOperationException();
        musicdata = new MusicDataInternal<float>(BufferSize, Channels);
    }

    public WavTrack(byte* buffer, int length)
    {
        bool success = DrWavLibrary.InitMemory(ref data, ref buffer[0], length, ref Unsafe.NullRef<AllocationCallbacks>());
        if (!success) throw new InvalidOperationException();
        Init(out frameCount);
    }

    private void Init(out ulong frameCount)
    {
        var result = DrWavLibrary.GetLengthPcmFrames(ref data, out frameCount);
        if (result != DrwavResult.Success) throw new InvalidOperationException();
        musicdata = new MusicDataInternal<float>(BufferSize, Channels);
    }

    public int SampleRateHz => (int)data.SampleRate;

    public TimeSpan Length => TimeSpan.FromSeconds((double)frameCount/ SampleRateHz);

    public TimeSpan CurrentPosition => TimeSpan.FromSeconds((double)currentFrame / SampleRateHz);

    public int Channels => data.Channels;

    private const int BufferSize = 4096;

    public bool AdvanceMusicData()
    {
        ulong framesRead = DrWavLibrary.ReadPcmFrames(ref data, BufferSize, ref musicdata.Ref);
        musicdata.Length = (int)framesRead;
        return framesRead > 0;
    }

    public MusicData<float> GetCurrentMusicData()
    {
        return musicdata.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        ulong sample = (ulong)(data.SampleRate * span.TotalSeconds);
        currentFrame = sample;
        bool success = DrWavLibrary.SeekToPcmFrame(ref data, sample);
        if (!success) throw new InvalidOperationException();
    }

    public void Signal(int signal)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            DrWavLibrary.Uninit(ref data);
            musicdata.Unallocate();
            disposedValue = true;
        }
    }

    ~WavTrack()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

