namespace Nemonuri.ManagedPointers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct DangerousSpanSnapshot<T>
{
    private readonly nint _pointer;
    private readonly int _length;

    internal DangerousSpanSnapshot(nint pointer, int length)
    {
        _pointer = pointer;
        _length = length;
    }

    public unsafe DangerousSpanSnapshot(ReadOnlySpan<T> readOnlySpan) : this
    (
        pointer: (nint)Unsafe.AsPointer(ref MemoryMarshal.GetReference(readOnlySpan)),
        length: readOnlySpan.Length
    )
    {}

    public nint Pointer => _pointer;

    public int Length => _length;

    public unsafe Span<T> GetSpan() => new ((void*)Pointer, Length);
}