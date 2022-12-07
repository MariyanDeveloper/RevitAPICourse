using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class TransverseLeftTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisX.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }
}