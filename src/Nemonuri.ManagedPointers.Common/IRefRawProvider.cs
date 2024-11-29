namespace Nemonuri.ManagedPointers;

public interface IRefRawProvider<TRaw>
{
    [UnscopedRef]
    ref TRaw Raw {get;}
}
