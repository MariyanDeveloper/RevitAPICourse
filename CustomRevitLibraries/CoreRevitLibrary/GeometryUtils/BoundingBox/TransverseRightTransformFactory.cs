using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class TransverseRightTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisX.ToTransform();
            return AdaptTransform(transform);
        }
    }
}