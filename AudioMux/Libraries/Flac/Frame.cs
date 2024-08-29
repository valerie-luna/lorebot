namespace AudioMux.Libraries.Flac;

public unsafe struct Frame
{
    public readonly FrameHeader Header;
    public readonly Subframe Subframes_1;
    public readonly Subframe Subframes_2;
    public readonly Subframe Subframes_3;
    public readonly Subframe Subframes_4;
    public readonly Subframe Subframes_5;
    public readonly Subframe Subframes_6;
    public readonly Subframe Subframes_7;
    public readonly Subframe Subframes_8;
    public readonly FrameFooter Footer;
}
