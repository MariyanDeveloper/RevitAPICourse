using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace CoreRevitLibrary.GeometryUtils
{
    public class Cuboid : IGeometryShape
    {
        public Solid ToSolid(AdvancedBoundingBoxXYZ advancedBoundingBoxXYZ)
        {
            var curveLoop = advancedBoundingBoxXYZ.BaseCurveLoop;
            var solid = GeometryCreationUtilities.CreateExtrusionGeometry(
                new List<CurveLoop>() { curveLoop },
                advancedBoundingBoxXYZ.Transform.BasisZ,
                advancedBoundingBoxXYZ.Height);
            return solid;

        }
    }
}