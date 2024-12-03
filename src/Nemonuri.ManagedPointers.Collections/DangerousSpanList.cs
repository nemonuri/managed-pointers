namespace Nemonuri.ManagedPointers.Collections;

public readonly ref partial struct DangerousSpanList<T>
{
    private readonly ReadOnlySpan<DangerousSpanSnapshot<T>> _innerDangerousSpanSnapshotSpan;

    public DangerousSpanList(ReadOnlySpan<DangerousSpanSnapshot<T>> innerDangerousSpanSnapshotSpan)
    {
        _innerDangerousSpanSnapshotSpan = innerDangerousSpanSnapshotSpan;
    }

    public  ReadOnlySpan<DangerousSpanSnapshot<T>> InnerDangerousSpanSnapshotSpan => _innerDangerousSpanSnapshotSpan;

    public int Count => _innerDangerousSpanSnapshotSpan.Length;

    public Span<T> this[int index]
    {
        get
        {
            Guard.IsInRange(index, 0, Count);
            return _innerDangerousSpanSnapshotSpan[index].GetSpan();
        }
    }

    public Enumerator GetEnumerator() => new (this);
}