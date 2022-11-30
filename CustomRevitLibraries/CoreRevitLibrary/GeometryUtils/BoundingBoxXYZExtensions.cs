using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public static class BoundingBoxXYZExtensions
    {
        public static AdvancedBoundingBoxXYZ ToAdvanced(this BoundingBoxXYZ box)
        {
            return AdvancedBoundingBoxXYZ.Create(box);
        }
    }
}
