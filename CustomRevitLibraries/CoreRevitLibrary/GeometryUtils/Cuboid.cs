using System.Collections.Generic;
using Autodesk.Revit.DB;

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