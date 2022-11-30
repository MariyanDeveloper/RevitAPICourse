using System;
using Autodesk.Revit.DB;
using CoreRevitLibrary.Enums;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.VectorExercises
{
    public class CurveConnector : ICurveConnector
    {
        private readonly ITriangleCalculation _triangleCalculation;
        public CurveConnector(ITriangleCalculation triangleCalculation)
        {
            _triangleCalculation = triangleCalculation;

        }
        public IConnectionResult Connect(Curve fromCurve, Curve toCurve, double angle)
        {
            var fromVector = fromCurve.ToNormalizedVector();
            var vertices = fromCurve.GetVertices();
            vertices.AddRange(toCurve.GetVertices());
            var closestPoints = vertices.GetClosestPointsAlongVector(fromVector);
            var furthestPoints = vertices.GetFurthestPointsAlongVector(fromVector);
            var verticalLeg = _triangleCalculation.VerticalLeg;
            var horizontalLeg = _triangleCalculation.HorizontalLeg;
            var requiredHorizontalLeg = verticalLeg * Math.Tan(angle);
            var difference = horizontalLeg - requiredHorizontalLeg;
            var curveToExtend = Line.CreateBound(furthestPoints.Item1, closestPoints.Item1);
            var clonedExtended = curveToExtend.ExtendAsCloned(difference, Extension.End);
            var centerCurve = Line.CreateBound(
                clonedExtended.GetEndPoint(1),
                closestPoints.Item2);
            return new ConnectionResult(clonedExtended, toCurve, centerCurve);

        }

    }
}