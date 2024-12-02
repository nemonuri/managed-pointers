namespace Nemonuri.ManagedPointers.Collections;

public readonly ref partial struct DangerousSpanList<T>
{
    public ref struct Enumerator
    {
        private readonly DangerousSpanList<T> _dangerousSpanList;
        private int _currentIndex;

        public Enumerator(DangerousSpanList<T> dangerousSpanList)
        {
            _dangerousSpanList = dangerousSpanList;
            _currentIndex = -1;
        }

        public bool MoveNext()
        {
            int index = _currentIndex + 1;
            if (index < _dangerousSpanList.Count)
            {
                _currentIndex = index;
                return true;
            }

            return false;
        }

        public Span<T> Current => _dangerousSpanList[_currentIndex];
    }
}