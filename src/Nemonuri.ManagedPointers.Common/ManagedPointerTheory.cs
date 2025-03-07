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

    public static bool TryGetSubSegment
    (
        ReadOnlySpan<nint> subSegmentsLengths,
        nint firstSubSegmentBasedDisplacement,
        out int foundSubSegmentIndex,
        out nint foundSubSegmentLength,
        out nint foundSubSegmentBasedDisplacement
    )
    {
        Guard.IsGreaterThanOrEqualTo(firstSubSegmentBasedDisplacement, 0);
        nint remainedDisplacement = firstSubSegmentBasedDisplacement;

        for (int i = 0; i < subSegmentsLengths.Length; i++)
        {
            nint currentSubSegmentLength = subSegmentsLengths[i];
            Guard.IsGreaterThanOrEqualTo(currentSubSegmentLength, 0);
            if (remainedDisplacement < currentSubSegmentLength)
            {
                foundSubSegmentIndex = i;
                foundSubSegmentLength = currentSubSegmentLength;
                foundSubSegmentBasedDisplacement = remainedDisplacement;
                return true;
            }
            else
            {
                remainedDisplacement -= currentSubSegmentLength;
            }
        }

        SetFailedResults(out foundSubSegmentIndex, out foundSubSegmentLength, out foundSubSegmentBasedDisplacement);
        return false;

        static void SetFailedResults(out int subSegmentIndex, out nint subSegmentLength, out nint subSegmentBasedDisplacement)
        {
            subSegmentIndex = default;
            subSegmentLength = default;
            subSegmentBasedDisplacement = default;
        }
    }

    public static bool TryGetSegmentTreeLayout<T>
    (
        out int treeBreadth,
        out int treeHeight
    )
    {
    }

    public static bool TryGetSegmentTree<T>
    (
        int treeBreadth,
        int treeHeight,

        Span<nint> subSegmentsLengthsFlattenedTreeDestination,
        Span<nint> subSegmentsDegreesFlattenedTreeDestination,
        Span<nint> leafSegmentsLengthsDestination
    )
    {
    }

    public static bool TryGetSubSegments<T>
    (
        int treeBreadth,
        int treeHeight,
        ReadOnlySpan<nint> subSegmentsLengthsFlattenedTree,
        ReadOnlySpan<nint> subSegmentsDegreesFlattenedTree,
        nint firstSubSegmentBasedDisplacement,

        Span<int> foundSubSegmentIndexsDestination,
        Span<nint> foundSubSegmentLengthsDestination,
        Span<nint> foundSubSegmentBasedDisplacementsDestination
    )
    {

    }

    public static bool TryGetLogTextForSegmentTree<T>
    (
        int treeBreadth,
        int treeHeight,
        ReadOnlySpan<nint> subSegmentsLengthsFlattenedTree,
        ReadOnlySpan<nint> subSegmentsDegreesFlattenedTree,
        ReadOnlySpan<nint> leafSegmentsLengths,

        out string resultLogText
    )
    {
        
    }
}
