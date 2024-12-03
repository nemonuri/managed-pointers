using Nemonuri.Monoids;

namespace Nemonuri.ManagedPointers.Collections;

public static partial class DangerousSpanPartitionTheory
{
    public readonly ref struct DangerousSpanListFactory<T>
    {
        public ReadOnlySpan<T> Entire {get;}

        public ISpanPartitionPremise<T> SpanPartitionPremise {get;}

        public int DangerousSpanSnapshotSpanLength {get;}

        public DangerousSpanListFactory(ReadOnlySpan<T> entire, ISpanPartitionPremise<T> spanPartitionPremise)
        {
            Entire = entire;
            SpanPartitionPremise = spanPartitionPremise;
            DangerousSpanSnapshotSpanLength = spanPartitionPremise.GetOutRangesSpanLength(entire);
        }

        public DangerousSpanList<T> Create(Span<DangerousSpanSnapshot<T>> dangerousSpanSnapshotSpan)
        {
            Guard.IsEqualTo(DangerousSpanSnapshotSpanLength, dangerousSpanSnapshotSpan.Length);

            Span<Range> rangeSpan = stackalloc Range[DangerousSpanSnapshotSpanLength];

            SpanPartitionPremise.Partition(Entire, rangeSpan);
            for (int i = 0; i < rangeSpan.Length; i++)
            {
                dangerousSpanSnapshotSpan[i] = new DangerousSpanSnapshot<T>(Entire[rangeSpan[i]]);
            }

            return new DangerousSpanList<T>(dangerousSpanSnapshotSpan);
        }
    }
}