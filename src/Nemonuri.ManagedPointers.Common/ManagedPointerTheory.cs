namespace Nemonuri.ManagedPointers;

public static class ManagedPointerTheory
{
    public static unsafe nint GetMemoryDisplacement<TGround, TTarget>
    (
        ref readonly TGround ground,
        ref readonly TTarget target
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ground));
        nint targetAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in target));
        return targetAddress - groundAddress;
    }

    public static unsafe ref TTarget GetFromMemoryDisplacement<TGround, TTarget>
    (
        scoped ref readonly TGround ground,
        nint memoryDisplacement
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ground));
        nint targetAddress = groundAddress + memoryDisplacement;
        return ref Unsafe.AsRef<TTarget>((void*)targetAddress);
    }

    public static void Method1
    (
        ReadOnlySpan<nint> layeredLengths,

        out int index,
        out nint length,
        out nint offset
    )
    {

    }
}
