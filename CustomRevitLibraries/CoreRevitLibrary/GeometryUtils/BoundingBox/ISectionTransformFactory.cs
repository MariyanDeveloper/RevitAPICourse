using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public interface ISectionTransformFactory
    {
        Transform CreateTransform(XYZ facingVector);
    }
}