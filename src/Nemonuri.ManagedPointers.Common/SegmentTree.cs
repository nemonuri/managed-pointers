namespace Nemonuri.ManagedPointers;

public readonly ref struct SegmentTree
{
    internal SegmentTree
    (
        Type rootType,
        int treeBreadth,
        int treeHeight,
        int maxTreeWidth,
        ReadOnlyTreeMemory2D<nint> subSegmentsLengthsTreeMemory2D,
        ReadOnlyTreeMemory2D<nint> subSegmentsDegreesTreeMemory2D,
        ReadOnlyTreeMemory2D<nint> subSegmentsFirstChildIndexesTreeMemory2D,
        ReadOnlyTreeMemory2D<nint> subSegmentsParentIndexesTreeMemory2D,
        ReadOnlySpan<nint> leafSegmentsLengths,
        ReadOnlySpan<int> leafSegmentsXInTreeMemory2D,
        ReadOnlySpan<int> leafSegmentsYInTreeMemory2D
    )
    {
        RootType = rootType;
        TreeBreadth = treeBreadth;
        TreeHeight = treeHeight;
        MaxTreeWidth = maxTreeWidth;

        SubSegmentsLengthsTreeMemory2D = subSegmentsLengthsTreeMemory2D;
        SubSegmentsDegreesTreeMemory2D = subSegmentsDegreesTreeMemory2D;
        SubSegmentsFirstChildIndexesTreeMemory2D = subSegmentsFirstChildIndexesTreeMemory2D;
        SubSegmentsParentIndexesTreeMemory2D = subSegmentsParentIndexesTreeMemory2D;
        LeafSegmentsLengths = leafSegmentsLengths;
        LeafSegmentsXInTreeMemory2D = leafSegmentsXInTreeMemory2D;
        LeafSegmentsYInTreeMemory2D = leafSegmentsYInTreeMemory2D;
    }

    public Type RootType {get;}
    public int TreeBreadth {get;}
    public int TreeHeight {get;}
    public int MaxTreeWidth {get;}
    public ReadOnlyTreeMemory2D<nint> SubSegmentsLengthsTreeMemory2D {get;}
    public ReadOnlyTreeMemory2D<nint> SubSegmentsDegreesTreeMemory2D {get;}
    public ReadOnlyTreeMemory2D<nint> SubSegmentsFirstChildIndexesTreeMemory2D {get;}
    public ReadOnlyTreeMemory2D<nint> SubSegmentsParentIndexesTreeMemory2D {get;}

    public ReadOnlySpan<nint> LeafSegmentsLengths {get;}
    public ReadOnlySpan<int> LeafSegmentsXInTreeMemory2D {get;}
    public ReadOnlySpan<int> LeafSegmentsYInTreeMemory2D {get;}
}