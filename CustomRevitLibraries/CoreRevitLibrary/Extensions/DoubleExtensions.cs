using System;

namespace CoreRevitLibrary.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToRadians(
            this double degree) => degree * (Math.PI / 180);

        public static double ToDegrees(
            this double radians) => radians * (180 / Math.PI);

        public static bool IsAlmostEqualTo(
            this double firstValue, double secondValue, double tolerance = 0.0001)

        {
            return Math.Abs(firstValue - secondValue) < tolerance;
        }
    }
}
