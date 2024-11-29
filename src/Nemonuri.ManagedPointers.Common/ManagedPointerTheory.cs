namespace Nemonuri.ManagedPointers;

public static class ManagedPointerTheory
{
    public static unsafe nint GetOffset<TGround, TTarget>
    (
        scoped ref readonly TGround refGround,
        scoped ref readonly TTarget refTarget
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in refGround));
        nint targetAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in refTarget));
        return targetAddress - groundAddress;
    }

    public static unsafe ref TTarget GetFromOffset<TGround, TTarget>
    (
        scoped ref readonly TGround refGround,
        nint offset
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in refGround));
        nint targetAddress = groundAddress + offset;
        return ref Unsafe.AsRef<TTarget>((void*)targetAddress);
    }
}
