namespace Nemonuri.ManagedPointers;

public struct SegmentTreeFactory
{
    public SegmentTreeFactory(Type rootType)
    {
        Guard.IsNotNull(rootType);
        RootType = rootType;

        ManagedPointerTheory.GetSegmentTreeLayout
        (
            RootType,
            out _treeBreadth,
            out _treeHeight,
            out _maxTreeWidth
        );
    }

    public SegmentTree Create(Span<nint> values, Span<int> indexes)
    {
        Guard.IsGreaterThanOrEqualTo(values.Length, RequiredLengthForValues);
        Guard.IsGreaterThanOrEqualTo(indexes.Length, RequiredLengthForValues);

        var remainedValues = values;
        var remainedIndexes = indexes;

        //--- Slice and assign ---
        //  --- Target: values ---
        TreeMemory2D<nint> subSegmentsLengthsTreeMemory2D = new(remainedValues, _maxTreeWidth, _treeHeight);
        remainedValues = remainedValues[subSegmentsLengthsTreeMemory2D.Count..];

        TreeMemory2D<nint> subSegmentsDegreesTreeMemory2D = new(remainedValues, _maxTreeWidth, _treeHeight);
        remainedValues = remainedValues[subSegmentsDegreesTreeMemory2D.Count..];

        TreeMemory2D<nint> subSegmentsFirstChildIndexesTreeMemory2D = new(remainedValues, _maxTreeWidth, _treeHeight);
        remainedValues = remainedValues[subSegmentsFirstChildIndexesTreeMemory2D.Count..];

        TreeMemory2D<nint> subSegmentsParentIndexesTreeMemory2D = new(remainedValues, _maxTreeWidth, _treeHeight);
        remainedValues = remainedValues[subSegmentsParentIndexesTreeMemory2D.Count..];

        Span<nint> leafSegmentsLengths = remainedValues[.._treeBreadth];
        //  ---|
        
        //  --- Target: indexes ---
        Span<int> leafSegmentsXInTreeMemory2D = remainedIndexes[.._treeBreadth];
        remainedIndexes = remainedIndexes[_treeBreadth..];

        Span<int> leafSegmentsYInTreeMemory2D = remainedIndexes[.._treeBreadth];
        //  ---|
        //---|

        ManagedPointerTheory.GetSegmentTree
        (
            RootType,
            _treeBreadth,
            _treeHeight,
            _maxTreeWidth,

            subSegmentsLengthsTreeMemory2D,
            subSegmentsDegreesTreeMemory2D,
            subSegmentsFirstChildIndexesTreeMemory2D,
            subSegmentsParentIndexesTreeMemory2D,

            leafSegmentsLengths,
            leafSegmentsXInTreeMemory2D,
            leafSegmentsYInTreeMemory2D,

            out _
        );

        return new SegmentTree
        (
            RootType,
            _treeBreadth,
            _treeHeight,
            _maxTreeWidth,

            subSegmentsLengthsTreeMemory2D,
            subSegmentsDegreesTreeMemory2D,
            subSegmentsFirstChildIndexesTreeMemory2D,
            subSegmentsParentIndexesTreeMemory2D,

            leafSegmentsLengths,
            leafSegmentsXInTreeMemory2D,
            leafSegmentsYInTreeMemory2D
        );
    }

    public Type RootType {get;}

    private int _treeBreadth;
    public readonly int TreeBreadth => _treeBreadth;

    private int _treeHeight;
    public readonly int TreeHeight => _treeHeight;

    private int _maxTreeWidth;
    public readonly int MaxTreeWidth => _maxTreeWidth;

    private int _requiredLengthForValues = -1;
    public int RequiredLengthForValues
    {
        get
        {
            if (_requiredLengthForValues < 0)
            {
                int flattenedTreeSpanLength = ManagedPointerTheory.GetRequiredLengthForFlattenedTreeSpan(_maxTreeWidth, _treeHeight);
                _requiredLengthForValues = flattenedTreeSpanLength * 4;
                _requiredLengthForValues += _treeBreadth;
            }

            return _requiredLengthForValues;
        }
    }

    private int _requiredLengthForIndexes = -1;
    public int RequiredLengthForIndexes
    {
        get
        {
            if (_requiredLengthForIndexes < 0)
            {
                _requiredLengthForIndexes = _treeBreadth * 4;
            }

            return _requiredLengthForIndexes;
        }
    }
}
