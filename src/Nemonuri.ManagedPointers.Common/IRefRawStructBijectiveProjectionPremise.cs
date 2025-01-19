namespace Nemonuri.ManagedPointers;

public interface IRefRawStructBijectiveProjectionPremise
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
    IRefRawStructProjectionPremise<TRawDomain, TRawCodomain> Projection {get;}

    IRefRawStructProjectionPremise<TRawCodomain, TRawDomain> InverseProjection {get;}
}