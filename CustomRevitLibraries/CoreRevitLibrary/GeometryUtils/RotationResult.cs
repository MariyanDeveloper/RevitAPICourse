namespace CoreRevitLibrary.GeometryUtils
{
    public class RotationResult : IRotationResult
    {
        public double AroundX { get; }
        public double AroundY { get; }
        public double AroundZ { get; }

        public RotationResult(double aroundX, double aroundY, double aroundZ)
        {
            AroundX = aroundX;
            AroundY = aroundY;
            AroundZ = aroundZ;
        }
    }
}