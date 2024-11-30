namespace Nemonuri.ManagedPointers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct RelativeRefSnapshot<T>
{
    private readonly nint _offset;

    public RelativeRefSnapshot(nint offset)
    {
        _offset = offset;
    }

    public nint Offset => _offset;

    public unsafe DangerousRefSnapshot<T> GetDangerousRefSnapshot(void* groundPointer)
    {
        nint resultPointer = (nint)groundPointer + _offset;
        return new DangerousRefSnapshot<T>(resultPointer);
    }

    public unsafe DangerousRefSnapshot<T> GetDangerousRefSnapshot(ref readonly T groundRef) => 
        GetDangerousRefSnapshot(Unsafe.AsPointer(ref Unsafe.AsRef(in groundRef)));

    public RelativeSpanSnapshot<T> ToRelativeSpanSnapshot() => new(_offset, 1);

    public static implicit operator RelativeSpanSnapshot<T>(RelativeRefSnapshot<T> value) => value.ToRelativeSpanSnapshot();
}