using Autodesk.Revit.DB;

namespace CoreRevitLibrary.GeometryUtils
{
    public class SectionBoundingBoxXYZ : AdvancedBoundingBoxXYZ
    {
        public SectionBoundingBoxXYZ(
            XYZ origin,
            XYZ facingVector,
            double locationLength,
            double height,
            double farClipOffset, ISectionTransformFactory transformFactory)
            : base(locationLength, height, farClipOffset)
        {
            var transform = transformFactory.CreateTransform(facingVector);
            transform.Origin = origin;
            Transform = transform;
        }

        //public SectionBoundingBoxXYZ(
        //    Line line, double height, double farClipOffset,
        //    ISectionTransformFactory transformFactory)
        //: base(line.Length, height, farClipOffset)
        //{

        //}
    }
}