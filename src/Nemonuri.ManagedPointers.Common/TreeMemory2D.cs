namespace Nemonuri.ManagedPointers;

public readonly ref struct TreeMemory2D<T>
{
    public TreeMemory2D(Span<T> source, int width, int height)
    {
        Guard.IsGreaterThan(width, 0);
        Guard.IsGreaterThan(height, 0);

        int requiredSpanLength = width * height;
        Guard.IsGreaterThanOrEqualTo(source.Length, requiredSpanLength);

        InnerSpan = source[..requiredSpanLength];
        Width = width;
        Height = height;    
    }

    public Span<T> InnerSpan {get;}
    public int Width {get;}
    public int Height {get;}

    public int Count => InnerSpan.Length;

    public ref T this[int x, int y] => ref InnerSpan[Index2DTheory.GetIndex1D(Width, x, y)];

    public ReadOnlyTreeMemory2D<T> AsReadOnly() => new (InnerSpan, Width, Height);

    public static implicit operator ReadOnlyTreeMemory2D<T>(TreeMemory2D<T> self) => self.AsReadOnly();
}
