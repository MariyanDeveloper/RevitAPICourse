using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public interface IGeometryShape
    {
        Solid ToSolid(
            AdvancedBoundingBoxXYZ advancedBoundingBoxXYZ);
    }
}