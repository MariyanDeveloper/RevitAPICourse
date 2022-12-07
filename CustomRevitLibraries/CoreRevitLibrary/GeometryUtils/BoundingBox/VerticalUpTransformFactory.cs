using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class VerticalUpTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisZ.ToTransform();
            return AdaptTransform(transform);
        }
    }
}