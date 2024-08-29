using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AudioMux;

public readonly unsafe ref struct MusicData<T>
    where T : unmanaged
{
    public readonly int Length;
    private readonly ref T Ref;
    private readonly int channels;

    public MusicData(int length, ref T left, int channels)
    {
        Length = length;
        Ref = ref left;
        this.channels = channels;
    }
    
    public readonly int BufferLength => Length * channels;

    public readonly Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Ref, BufferLength);

    public readonly ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index >= ((uint)BufferLength))
                IndexOutOfRangeException();
            return ref Unsafe.Add(ref Ref, index);
        }
    }

    private static void IndexOutOfRangeException() => throw new IndexOutOfRangeException();
}

public unsafe struct MusicDataInternal<T>
    where T : unmanaged
{
    public int Length;
    private T* Ptr;
    public readonly ref T Ref => ref (*Ptr);
    private readonly int channels;

    public readonly int BufferLength => Length * channels;

    public MusicDataInternal(int channels)
    {
        Length = 0;
        this.channels = channels;
        Ptr = null;
    }

    public MusicDataInternal(int length, int channels)
    {
        Length = length;
        this.channels = channels;
        Ptr = Utilities.Allocate<T>(length * channels);
    }

    public void Unallocate()
    {
        Length = 0;
        Utilities.Free(Ptr);
        Ptr = null;
    }

    public void Reallocate(int length)
    {
        Unallocate();
        Length = length;
        Ptr = Utilities.Allocate<T>(length * channels);
    }

    public readonly Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Ref, BufferLength);

    public readonly MusicData<T> ToMusicData() => new MusicData<T>(Length, ref Ref, channels);

    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index >= ((uint)BufferLength))
                IndexOutOfRangeException();
            return ref Unsafe.Add(ref Ref, index);
        }
    }

    private static void IndexOutOfRangeException() => throw new IndexOutOfRangeException();
}