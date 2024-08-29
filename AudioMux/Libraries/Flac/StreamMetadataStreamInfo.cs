namespace AudioMux.Libraries.Flac;

public readonly struct StreamMetadataStreamInfo
{
    private readonly uint type;
    private readonly uint last;
    private readonly uint length;
    private readonly uint padding;
    public readonly uint MinBlocksize;
    public readonly uint MaxBlocksize;
    public readonly uint MinFramesize;
    public readonly uint MaxFramesize;
    public readonly uint SampleRate;
    public readonly uint Channels;
    public readonly uint BitsPerSample;
    public readonly ulong TotalSamples;
}
