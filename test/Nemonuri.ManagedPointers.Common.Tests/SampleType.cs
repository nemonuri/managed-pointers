namespace Nemonuri.ManagedPointers.Common.Tests;


public struct SampleType1
{
    public int X;
}

public struct SampleType2
{
    public int X;
    public int Y;
}

public struct SampleType3
{
    public SampleType2 SampleType2_1;
    public long Middle;
    public SampleType2 SampleType2_2;
}

public struct SampleType4
{
    public SampleType3 SampleType3_1;
}

public struct SampleType5
{
    public SampleType3 SampleType3_1;
    public SampleType4 SampleType4_1;
}