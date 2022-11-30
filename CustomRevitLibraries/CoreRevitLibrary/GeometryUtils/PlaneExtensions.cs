using Autodesk.Revit.DB;
using CoreRevitLibrary.Extensions;
using System.Collections.Generic;

namespace CoreRevitLibrary.GeometryUtils
{
    public static class PlaneExtensions
    {
        public static Transform ToTransform(this Plane plane, Orientation orientation = Orientation.Facing)
        {
            var transform = Transform.Identity;
            if (orientation == Orientation.Facing)
            {
                transform.BasisY = plane.Normal;
                transform.BasisX = plane.XVec;
                transform.BasisZ = plane.YVec;
            }
            else
            {
                transform.BasisX = plane.Normal;
                transform.BasisY = plane.XVec;
                transform.BasisZ = plane.YVec;
            }
            transform.Origin = plane.Origin;
            return transform;
        }

        public static void Visualize(
            this Plane plane, Document doc, int scale = 3)
        {
            var planeOrigin = plane.Origin;
            var upperRightCorner = planeOrigin + (plane.XVec * scale) + (plane.YVec * scale);
            var upperLeftCorner = planeOrigin - (plane.XVec * scale) + (plane.YVec * scale);
            var bottomRightCorner = planeOrigin + (plane.XVec * scale) - (plane.YVec * scale);
            var bottomLeftCorner = planeOrigin - (plane.XVec * scale) - (plane.YVec * scale);
            var curves = new List<GeometryObject>();
            curves.Add(Line.CreateBound(
                upperRightCorner, upperLeftCorner));
            curves.Add(Line.CreateBound(
                upperRightCorner, bottomRightCorner));
            curves.Add(Line.CreateBound(
                upperLeftCorner, bottomLeftCorner));
            curves.Add(Line.CreateBound(
                bottomLeftCorner, bottomRightCorner));
            curves.Add(Line.CreateBound(
                planeOrigin, planeOrigin + plane.Normal));
            doc.CreateDirectShape(curves);
        }
    }
}
