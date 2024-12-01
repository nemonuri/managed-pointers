using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Nemonuri.ManagedPointers.Collections;

public ref struct DangerousSpanList<T>
{
    private readonly ReadOnlySpan<DangerousSpanSnapshot<T>> _innerDangerousSpanSnapshotSpan;

    public DangerousSpanList(in ReadOnlySpan<DangerousSpanSnapshot<T>> innerDangerousSpanSnapshotSpan)
    {
        _innerDangerousSpanSnapshotSpan = innerDangerousSpanSnapshotSpan;
    }

    public readonly ReadOnlySpan<DangerousSpanSnapshot<T>> InnerDangerousSpanSnapshotSpan => _innerDangerousSpanSnapshotSpan;

    public readonly int Count => _innerDangerousSpanSnapshotSpan.Length;

    public Span<T> this[int index]
    {
        get
        {
            Guard.IsInRange(index, 0, Count);
            return _innerDangerousSpanSnapshotSpan[index].GetSpan();
        }
    }
}