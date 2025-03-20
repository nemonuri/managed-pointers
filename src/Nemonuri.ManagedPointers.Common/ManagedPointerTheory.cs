using System.Reflection;
using System.Buffers;

namespace Nemonuri.ManagedPointers;

public static class ManagedPointerTheory
{
    public static unsafe nint GetMemoryDisplacement<TGround, TTarget>
    (
        ref readonly TGround ground,
        ref readonly TTarget target
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ground));
        nint targetAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in target));
        return targetAddress - groundAddress;
    }

    public static unsafe ref TTarget GetFromMemoryDisplacement<TGround, TTarget>
    (
        scoped ref readonly TGround ground,
        nint memoryDisplacement
    )
    {
        nint groundAddress = (nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ground));
        nint targetAddress = groundAddress + memoryDisplacement;
        return ref Unsafe.AsRef<TTarget>((void*)targetAddress);
    }

    public static bool TryGetSubSegment
    (
        ReadOnlySpan<nint> subSegmentsLengths,
        nint firstSubSegmentBasedDisplacement,
        out int foundSubSegmentIndex,
        out nint foundSubSegmentLength,
        out nint foundSubSegmentBasedDisplacement
    )
    {
        Guard.IsGreaterThanOrEqualTo(firstSubSegmentBasedDisplacement, 0);
        nint remainedDisplacement = firstSubSegmentBasedDisplacement;

        for (int i = 0; i < subSegmentsLengths.Length; i++)
        {
            nint currentSubSegmentLength = subSegmentsLengths[i];
            Guard.IsGreaterThanOrEqualTo(currentSubSegmentLength, 0);
            if (remainedDisplacement < currentSubSegmentLength)
            {
                foundSubSegmentIndex = i;
                foundSubSegmentLength = currentSubSegmentLength;
                foundSubSegmentBasedDisplacement = remainedDisplacement;
                return true;
            }
            else
            {
                remainedDisplacement -= currentSubSegmentLength;
            }
        }

        SetFailedResults(out foundSubSegmentIndex, out foundSubSegmentLength, out foundSubSegmentBasedDisplacement);
        return false;

        static void SetFailedResults(out int subSegmentIndex, out nint subSegmentLength, out nint subSegmentBasedDisplacement)
        {
            subSegmentIndex = default;
            subSegmentLength = default;
            subSegmentBasedDisplacement = default;
        }
    }

/**
# Tree Traversal 관련 용어들
- Preorder Traversal (DFT, Depth-First Traversal)
- Inorder Traversal (symmetric traversal)
- Postorder Traversal

(출처: https://yoongrammer.tistory.com/70)


# Tree 구조 관련 용어들
- The **height** of a **node** is the length of the longest downward path to a leaf from that node
- The **height** of the **root** is the height of the tree
- Breadth: The number of leaves

(출처: https://en.wikipedia.org/wiki/Tree_(abstract_data_type)#Terminology)
*/

    public static void GetSegmentTreeLayout
    (
        Type rootType,
        out int treeBreadth,
        out int treeHeight,
        out int maxTreeWidth
    )
    {
        GetSegmentTreeBreadthAndHeight(rootType, out treeBreadth, out treeHeight);
        GetSegmentTreeMaxWidth(rootType, treeHeight, out maxTreeWidth);
    }

    public static void GetSegmentTreeBreadthAndHeight
    (
        Type rootType,
        out int breadth,
        out int height
    )
    {
        int currentBreadth = 0;
        int highestDepth = 0;
        int currentDepth = 0;

        TraverseDepthFirst(rootType, ref currentBreadth, ref currentDepth, ref highestDepth);

        breadth = currentBreadth;
        height = highestDepth;

        static void TraverseDepthFirst(Type currentType, ref int currentBreadth, ref int currentDepth, ref int highestDepth)
        {
            //--- Get child nodes ---
            FieldInfo[] fields = currentType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            //---|

            //--- If node is leaf, increase breadth ---
            if (fields.Length == 0)
            {
                currentBreadth++;
                return;
            }
            //---|
            
            currentDepth++;
            highestDepth = Math.Max(highestDepth, currentDepth);

            //--- Traverse child nodes ---
            foreach (FieldInfo field in fields)
            {
                TraverseDepthFirst(field.FieldType, ref currentBreadth, ref currentDepth, ref highestDepth);
            }
            //---|
            
            currentDepth--;
        }
    }

    public static void GetSegmentTreeMaxWidth
    (
        Type rootType,
        int height,
        out int maxWidth
    )
    {
        Guard.IsGreaterThanOrEqualTo(height, 0);

        int currentDepth = 0;
        Span<int> currentWidthsPerDepth = stackalloc int[height+1];
        currentWidthsPerDepth.Clear();

        TraverseDepthFirst(rootType, ref currentDepth, currentWidthsPerDepth);

        maxWidth = 0;
        foreach (int width in currentWidthsPerDepth)    
        {
            maxWidth = Math.Max(width, maxWidth);
        }

        static void TraverseDepthFirst(Type currentType, ref int currentDepth, Span<int> currentWidthsPerDepth)
        {
            //--- Increase current width in current depth ---
            currentWidthsPerDepth[currentDepth]++;
            //---|

            //--- Get child nodes ---
            FieldInfo[] fields = currentType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            //---|
            
            currentDepth++;

            //--- Traverse child nodes ---
            foreach (FieldInfo field in fields)
            {
                TraverseDepthFirst(field.FieldType, ref currentDepth, currentWidthsPerDepth);
            }
            //---|
            
            currentDepth--;
        }
    }

    public static void GetSegmentTree
    (
        Type rootType,
        int treeBreadth,
        int treeHeight,
        int maxTreeWidth,

        Span<nint> subSegmentsLengthsFlattenedTreeDestination,
        Span<nint> subSegmentsDegreesFlattenedTreeDestination,
        Span<nint> subSegmentsFirstChildIndexesFlattenedTreeDestination,
        Span<nint> subSegmentsParentIndexesFlattenedTreeDestination,
        Span<nint> leafSegmentsLengthsDestination,
        out int requiredLengthForFlattenedTreeSpan
    )
    {
        requiredLengthForFlattenedTreeSpan = GetRequiredLengthForFlattenedTreeSpan(maxTreeWidth, treeHeight);
        Guard.IsLessThanOrEqualTo(requiredLengthForFlattenedTreeSpan, subSegmentsLengthsFlattenedTreeDestination.Length);
        Guard.IsLessThanOrEqualTo(requiredLengthForFlattenedTreeSpan, subSegmentsDegreesFlattenedTreeDestination.Length);
        Guard.IsLessThanOrEqualTo(requiredLengthForFlattenedTreeSpan, subSegmentsFirstChildIndexesFlattenedTreeDestination.Length);
        Guard.IsLessThanOrEqualTo(requiredLengthForFlattenedTreeSpan, subSegmentsParentIndexesFlattenedTreeDestination.Length);
        Guard.IsLessThanOrEqualTo(treeBreadth, leafSegmentsLengthsDestination.Length);

        //--- Initialize flattened tree spans ---
        subSegmentsLengthsFlattenedTreeDestination[..requiredLengthForFlattenedTreeSpan].Fill(-1);
        subSegmentsDegreesFlattenedTreeDestination[..requiredLengthForFlattenedTreeSpan].Fill(-1);
        subSegmentsFirstChildIndexesFlattenedTreeDestination[..requiredLengthForFlattenedTreeSpan].Fill(-1);
        subSegmentsParentIndexesFlattenedTreeDestination[..requiredLengthForFlattenedTreeSpan].Fill(-1);
        //---|

        Span<int> subSegmentsLengthsTreeCurrentWidthsPerDepth = stackalloc int[treeHeight+1];
        subSegmentsLengthsTreeCurrentWidthsPerDepth.Clear();

        Span<nint> subSegmentsAccumulatedDegreesPerDepth = stackalloc nint[treeHeight+1];
        subSegmentsAccumulatedDegreesPerDepth.Clear();

        int leafSegmentsLengthsWritingIndex = 0;

        TraverseDepthFirst
        (
            rootType,
            treeBreadth,
            treeHeight,
            0,
            0,

            subSegmentsLengthsFlattenedTreeDestination,
            subSegmentsLengthsTreeCurrentWidthsPerDepth,

            subSegmentsDegreesFlattenedTreeDestination,
            subSegmentsAccumulatedDegreesPerDepth,

            subSegmentsFirstChildIndexesFlattenedTreeDestination,

            subSegmentsParentIndexesFlattenedTreeDestination,

            leafSegmentsLengthsDestination,
            ref leafSegmentsLengthsWritingIndex
        );

        static void TraverseDepthFirst
        (
            Type currentType,
            int treeBreadth,
            int treeHeight,
            int currentDepth,
            int parentIndexInPreviousDepth,

            Span<nint> subSegmentsLengthsFlattenedTreeDestination,
            Span<int> subSegmentsLengthsTreeCurrentWidthsPerDepth,

            Span<nint> subSegmentsDegreesFlattenedTreeDestination,
            Span<nint> subSegmentsAccumulatedDegreesPerDepth,

            Span<nint> subSegmentsFirstChildIndexesFlattenedTreeDestination,

            Span<nint> subSegmentsParentIndexesFlattenedTreeDestination,

            Span<nint> leafSegmentsLengthsDestination,
            ref int leafSegmentsLengthsWritingIndex
        )
        {
            int currentIndexInCurrentDepth = subSegmentsLengthsTreeCurrentWidthsPerDepth[currentDepth];
            subSegmentsLengthsTreeCurrentWidthsPerDepth[currentDepth]++;

            //--- If current depth is more than 0, write node date: parent index ---
            if (currentDepth > 0)
            {
                GetRefAsFlattenedTree(subSegmentsParentIndexesFlattenedTreeDestination, treeBreadth, treeHeight, 
                    currentIndexInCurrentDepth, currentDepth) =
                    parentIndexInPreviousDepth;
            }
            //---|

            //--- Write node data: segment length ---
            GetRefAsFlattenedTree(subSegmentsLengthsFlattenedTreeDestination, treeBreadth, treeHeight, 
                currentIndexInCurrentDepth, currentDepth) =
                Marshal.SizeOf(currentType);
            //---|

            //--- Get child nodes ---
            FieldInfo[] fields = currentType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            //---|

            //--- Write node data: degree (child count) ---
            GetRefAsFlattenedTree(subSegmentsDegreesFlattenedTreeDestination, treeBreadth, treeHeight, 
                currentIndexInCurrentDepth, currentDepth) =
                fields.Length;
            //---|

            //--- If node is leaf, set leaf ---
            if (fields.Length == 0)
            {
                leafSegmentsLengthsDestination[leafSegmentsLengthsWritingIndex] = Marshal.SizeOf(currentType);
                leafSegmentsLengthsWritingIndex++;
                return;
            }
            //---|

            //--- Write node data: first child index in next ---
            GetRefAsFlattenedTree(subSegmentsFirstChildIndexesFlattenedTreeDestination, treeBreadth, treeHeight, 
                currentIndexInCurrentDepth, currentDepth) =
                subSegmentsAccumulatedDegreesPerDepth[currentDepth];
            subSegmentsAccumulatedDegreesPerDepth[currentDepth] += fields.Length;
            //---|
            
            //--- Traverse child nodes ---
            foreach (FieldInfo field in fields)
            {
                TraverseDepthFirst
                (
                    field.FieldType, 
                    treeBreadth,
                    treeHeight,
                    currentDepth+1,
                    currentIndexInCurrentDepth,

                    subSegmentsLengthsFlattenedTreeDestination,
                    subSegmentsLengthsTreeCurrentWidthsPerDepth,

                    subSegmentsDegreesFlattenedTreeDestination,
                    subSegmentsAccumulatedDegreesPerDepth,

                    subSegmentsFirstChildIndexesFlattenedTreeDestination,

                    subSegmentsParentIndexesFlattenedTreeDestination,

                    leafSegmentsLengthsDestination,
                    ref leafSegmentsLengthsWritingIndex
                );
            }
            //---|
        }
    }

    public static int GetRequiredLengthForFlattenedTreeSpan
    (
        int maxTreeWidth,
        int treeHeight
    )
    {
        return maxTreeWidth * (treeHeight + 1);
    }

    public static ref nint GetRefAsFlattenedTree(Span<nint> flattenedTree, int treeBreadth, int treeHeight, int x, int y)
    {
        Guard.IsLessThan(x, treeBreadth);
        Guard.IsLessThan(y, treeHeight+1);
        return ref flattenedTree[x + treeBreadth * y];
    }

#if false
    public static bool TryGetSubSegments<T>
    (
        int treeBreadth,
        int treeHeight,
        ReadOnlySpan<nint> subSegmentsLengthsFlattenedTree,
        ReadOnlySpan<nint> subSegmentsDegreesFlattenedTree,
        nint firstSubSegmentBasedDisplacement,

        Span<int> foundSubSegmentIndexsDestination,
        Span<nint> foundSubSegmentLengthsDestination,
        Span<nint> foundSubSegmentBasedDisplacementsDestination
    )
    {

    }

    public static bool TryGetLogTextForSegmentTree<T>
    (
        int treeBreadth,
        int treeHeight,
        ReadOnlySpan<nint> subSegmentsLengthsFlattenedTree,
        ReadOnlySpan<nint> subSegmentsDegreesFlattenedTree,
        ReadOnlySpan<nint> leafSegmentsLengths,

        out string resultLogText
    )
    {
        
    }
#endif
}
