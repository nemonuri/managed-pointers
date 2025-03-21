namespace Nemonuri.ManagedPointers;

public readonly ref struct ReadOnlyTreeMemory2D<T>
{
    public ReadOnlyTreeMemory2D(ReadOnlySpan<T> source, int width, int height)
    {
        Guard.IsGreaterThan(width, 0);
        Guard.IsGreaterThan(height, 0);

        int requiredSpanLength = width * height;
        Guard.IsGreaterThanOrEqualTo(source.Length, requiredSpanLength);

        InnerSpan = source[..requiredSpanLength];
        Width = width;
        Height = height;    
    }

    public ReadOnlySpan<T> InnerSpan {get;}
    public int Width {get;}
    public int Height {get;}

    public int Count => InnerSpan.Length;

    public ref readonly T this[int x, int y] => ref InnerSpan[Index2DTheory.GetIndex1D(Width, x, y)];
}
