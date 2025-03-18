using Xunit.Abstractions;
using CL = Nemonuri.LogTexts.CollectionLogTextTheory;

namespace Nemonuri.ManagedPointers.Common.Tests;

public class ManagedPointerTheoryTest
{
    private readonly ITestOutputHelper _output;

    public ManagedPointerTheoryTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [MemberData(nameof(Data1))]
    public void TypeName__Get_Type_And_Invoke_GetSegmentTreeLayout__Tree_Breadth_And_Height_Are_Expected
    (
        string typeName,
        int expectedTreeBreadth,
        int expectedTreeHeight
    )
    {
        // Model
        Type type = Type.GetType(typeName) ?? throw new ArgumentNullException();

        // Act
        ManagedPointerTheory.GetSegmentTreeLayout
        (
            type, 
            out int actualTreeBreadth, 
            out int actualTreeHeight
        );

        // Assert
        _output.WriteLine(
            $"""
            typeName: {typeName}
            expectedTreeBreadth: {expectedTreeBreadth},
            expectedTreeHeight: {expectedTreeHeight},
            actualTreeBreadth: {actualTreeBreadth},
            actualTreeHeight: {actualTreeHeight}
            """
        );
        Assert.Equal(expectedTreeBreadth, actualTreeBreadth);
        Assert.Equal(expectedTreeHeight, actualTreeHeight);
    }

    public static TheoryData<string, int, int> Data1 => new ()
    {
        {typeof(SampleType1).FullName!, 1, 1},
        {typeof(SampleType2).FullName!, 2, 1},
        {typeof(SampleType3).FullName!, 5, 2},
        {typeof(SampleType4).FullName!, 5, 3}
    };


    [Theory]
    [MemberData(nameof(Data2))]
    public void TypeName__GetSegmentTree__Lengths_Are_Expected
    (
        string typeName,
        int[] expectedSegmentsLengths
    )
    {
        // Model
        Type type = Type.GetType(typeName) ?? throw new ArgumentNullException();
        ManagedPointerTheory.GetSegmentTreeLayout(type, out int treeBreadth, out int treeHeight);
        int treeSize = ManagedPointerTheory.GetRequiredLengthForFlattenedTreeSpan(treeBreadth, treeHeight);
        nint[] expectedSegmentsLengthsAsNInts = expectedSegmentsLengths.Select(i => (nint)i).ToArray();
        
        Span<nint> subSegmentsLengthsFlattened = stackalloc nint[treeSize];
        Span<nint> subSegmentsDegreesFlattened = stackalloc nint[treeSize];
        Span<nint> subSegmentsFirstChildIndexesFlattenedTree = stackalloc nint[treeSize];
        Span<nint> subSegmentsParentIndexesFlattenedTree = stackalloc nint[treeSize];
        Span<nint> leafSegmentsLengths = stackalloc nint[treeBreadth];

        // Act
        ManagedPointerTheory.GetSegmentTree
        (
            rootType: type,
            treeBreadth: treeBreadth,
            treeHeight: treeHeight,

            subSegmentsLengthsFlattenedTreeDestination: subSegmentsLengthsFlattened,
            subSegmentsDegreesFlattenedTreeDestination: subSegmentsDegreesFlattened,
            subSegmentsFirstChildIndexesFlattenedTreeDestination: subSegmentsFirstChildIndexesFlattenedTree,
            subSegmentsParentIndexesFlattenedTreeDestination: subSegmentsParentIndexesFlattenedTree,
            leafSegmentsLengthsDestination: leafSegmentsLengths,

            requiredLengthForFlattenedTreeSpan: out _
        );

        // Assert
        _output.WriteLine(
            $"""
            expected: {CL.ToLogString(expectedSegmentsLengths)},
            actual: {CL.ToLogString(subSegmentsLengthsFlattened)}
            """
        );
        Assert.Equal(expectedSegmentsLengthsAsNInts, subSegmentsLengthsFlattened);
    }

    public static TheoryData<string, int[]> Data2 => new ()
    {
        {
            typeof(SampleType1).FullName!, [1] // [X]
        },
        {
            typeof(SampleType2).FullName!, [1, 1] // [X, Y]
        },
        {
            typeof(SampleType3).FullName!, 
            [
                2, 1, 2, /**/ -1, -1, // [SampleType2_1, Middle, SampleType2_2]
                1, 1, /**/ 1, 1, -1 // [X, Y, X, Y]
            ]
        }
    };
}
