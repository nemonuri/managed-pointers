namespace Nemonuri.ManagedPointers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct DangerousRefSnapshot<T>
{
    private readonly nint _pointer;

    internal DangerousRefSnapshot(nint pointer)
    {
        _pointer = pointer;
    }

    public unsafe DangerousRefSnapshot(ref readonly T glvalue) : this
    (
        pointer: (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in glvalue))
    )
    {}

    public nint Pointer => _pointer;

    public unsafe ref T GetRef() => ref Unsafe.AsRef<T>((void*)Pointer);
}
