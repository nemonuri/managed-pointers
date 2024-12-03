using Nemonuri.Monoids;

namespace Nemonuri.ManagedPointers.Collections;

public static partial class DangerousSpanPartitionTheory
{
    public static DangerousSpanListFactory<T> GetDangerousSpanListFactory<T>
    (
        ReadOnlySpan<T> entire, 
        ISpanPartitionPremise<T> spanPartitionPremise
    ) 
    =>
    new (entire, spanPartitionPremise);
}