namespace Nemonuri.ManagedPointers;

public interface IRefRawStructProjectionPremise
<
    TRawDomain,
    TRawCodomain
>
    where TRawDomain : 
        struct
#if NET9_0_OR_GREATER
        , allows ref struct
#endif

    where TRawCodomain :
        struct
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
{
    ref TRawCodomain Project(ref TRawDomain item);

    bool TryProject(ref TRawDomain item, out TRawCodomain outResult);
}
