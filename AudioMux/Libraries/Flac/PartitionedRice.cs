namespace AudioMux.Libraries.Flac;

public readonly unsafe struct PartitionedRice
{
    public readonly uint Order;
    public readonly PartitionedRiceContents* Contents;
}
