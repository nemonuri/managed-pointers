using Xunit.Abstractions;

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

        _output.WriteLine(
            $"""
            typeName: {typeName}
            expectedTreeBreadth: {expectedTreeBreadth},
            expectedTreeHeight: {expectedTreeHeight},
            actualTreeBreadth: {actualTreeBreadth},
            actualTreeHeight: {actualTreeHeight}
            """
        );
        // Assert
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

}
