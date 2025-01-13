namespace Nemonuri.ManagedPointers.Collections;

public ref struct SpanBasedStack<T>
{
    private readonly Span<T> _innerSpan;

    private int _currentIndex;

    public SpanBasedStack(Span<T> innerSpan)
    {
        _innerSpan = innerSpan;
        _currentIndex = 0;
    }

    public Span<T> InnerSpan => _innerSpan;

    public int CurrentIndex => _currentIndex;

    public int Length => _innerSpan.Length;

    public void Push(T item)
    {
        _innerSpan[_currentIndex] = item;
        _currentIndex++;
    }
}