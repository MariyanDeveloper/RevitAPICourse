using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class VerticalDownTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisZ.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }
}