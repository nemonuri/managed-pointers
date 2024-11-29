namespace Nemonuri.ManagedPointers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct DangerousSpanSnapshot<T>
{
    private readonly nint pointer;
    private readonly int length;

    internal DangerousSpanSnapshot(nint pointer, int length)
    {
        this.pointer = pointer;
        this.length = length;
    }

    public unsafe DangerousSpanSnapshot(in ReadOnlySpan<T> readOnlySpan) : this
    (
        pointer: (nint)Unsafe.AsPointer(ref MemoryMarshal.GetReference(readOnlySpan)),
        length: readOnlySpan.Length
    )
    {}

    public nint Pointer => pointer;

    public int Length => length;

    public unsafe Span<T> GetSpan() => new ((void*)Pointer, Length);
}