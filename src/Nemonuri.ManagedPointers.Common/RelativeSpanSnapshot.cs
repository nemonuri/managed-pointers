namespace Nemonuri.ManagedPointers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct RelativeSpanSnapshot<T>
{
    private readonly nint _offset;
    private readonly int _length;

    public RelativeSpanSnapshot(nint offset, int length)
    {
        _offset = offset;
        _length = length;
    }

    public nint Offset => _offset;

    public int Length => _length;

    public unsafe DangerousSpanSnapshot<T> GetDangerousSpanSnapshot(void* groundPointer)
    {
        nint resultPointer = (nint)groundPointer + _offset;
        return new DangerousSpanSnapshot<T>(resultPointer, _length);
    }

    public unsafe DangerousSpanSnapshot<T> GetDangerousSpanSnapshot(ref readonly T groundRef) => 
        GetDangerousSpanSnapshot(Unsafe.AsPointer(ref Unsafe.AsRef(in groundRef)));
}