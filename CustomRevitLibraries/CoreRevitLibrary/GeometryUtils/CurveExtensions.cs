using Autodesk.Revit.DB;
using CoreRevitLibrary.Enums;
using CoreRevitLibrary.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.GeometryUtils
{
    public static class CurveExtensions
    {
        public static void Visualize(this Curve curve, Document document)
        {
            document.CreateDirectShape(curve);
        }

        /// <summary>
        /// This method is used to get normalized vector by curve
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static XYZ ToNormalizedVector(
            this Curve curve)
        {
            return (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();
        }

        /// <summary>
        /// This method is used to get vector by curve
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static XYZ ToVector(
            this Curve curve)
        {
            return (curve.GetEndPoint(1) - curve.GetEndPoint(0));
        }

        public static List<XYZ> GetVertices(this Curve curve)
        {
            return curve.Tessellate().ToList();
        }

        public static void Extend(
            this Curve curve, double value, Extension extension = Extension.Both)
        {
            var startParameter = curve.GetEndParameter(0);
            var endParameter = curve.GetEndParameter(1);
            if (extension == Extension.Both)
            {
                curve.MakeBound(startParameter - value, endParameter + value);
            }
            else
            {
                if (extension == Extension.Start)
                {
                    curve.MakeBound(startParameter - value, endParameter);

                }
                else
                {
                    curve.MakeBound(startParameter, endParameter + value);
                }

            }
        }
        public static Curve ExtendAsCloned(
            this Curve curve, double value, Extension extension = Extension.Both)
        {
            var clonedCurve = curve.Clone();
            clonedCurve.Extend(value, extension);
            return clonedCurve;
        }

        public static XYZ GetCenter(this Curve curve) => curve.Evaluate(0.5, true);



    }
}
