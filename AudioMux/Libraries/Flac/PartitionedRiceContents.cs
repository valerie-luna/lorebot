namespace AudioMux.Libraries.Flac;

public readonly unsafe struct PartitionedRiceContents
{
    public readonly uint* Parameters;
    public readonly uint* RawBits;
    public readonly uint CapacityByOrder;
}
