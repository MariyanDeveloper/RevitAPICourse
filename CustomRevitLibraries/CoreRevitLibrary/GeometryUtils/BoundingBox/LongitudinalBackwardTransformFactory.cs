using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class LongitudinalBackwardTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }
}