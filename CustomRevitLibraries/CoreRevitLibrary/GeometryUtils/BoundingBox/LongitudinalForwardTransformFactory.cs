using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class LongitudinalForwardTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform();
            return AdaptTransform(transform);
        }
    }
}