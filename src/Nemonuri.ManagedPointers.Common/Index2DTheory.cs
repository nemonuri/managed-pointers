namespace Nemonuri.ManagedPointers;

public static class Index2DTheory
{
    public static int GetIndex1D(int matrixWidth, int x, int y) => x + matrixWidth * y;
}