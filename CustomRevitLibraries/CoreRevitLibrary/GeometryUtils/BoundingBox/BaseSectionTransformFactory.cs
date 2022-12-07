using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public abstract class BaseSectionTransformFactory : ISectionTransformFactory
    {
        public abstract Transform CreateTransform(XYZ facingVector);

        protected Transform AdaptTransform(Transform transformToAdapt)
        {
            var outputTransform = Transform.Identity;
            outputTransform.BasisZ = transformToAdapt.BasisY;
            outputTransform.BasisY = transformToAdapt.BasisZ;
            outputTransform.BasisX = -transformToAdapt.BasisX;
            return outputTransform;
        }

    }
}