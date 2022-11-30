using System;
using Autodesk.Revit.DB;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.VectorExercises
{
    public class TriangleCalculation : ITriangleCalculation
    {
        private Curve _fromCurve { get; }
        private Curve _toCurve { get; }
        public double VerticalLeg => GetVectorBtwCurves().GetLength();
        public double HorizontalLeg => GetHorizontalLeg();
        private XYZ _fromCurveVector => _fromCurve.ToNormalizedVector();

        public TriangleCalculation(Curve fromCurve, Curve toCurve)
        {
            _fromCurve = fromCurve ?? throw new ArgumentNullException(nameof(fromCurve));
            _toCurve = toCurve ?? throw new ArgumentNullException(nameof(toCurve));
        }
        private XYZ GetVectorBtwCurves()
        {
            var fromCenter = _fromCurve.Evaluate(0.5, true);
            var toCenter = _toCurve.Evaluate(0.5, true);
            var distance = toCenter.GetSignedDistance(fromCenter, _fromCurveVector);
            var alignedPoint = toCenter.MoveAlongVector(_fromCurveVector * distance);
            var vectorBtwCurves = fromCenter.ToVector(alignedPoint);
            return vectorBtwCurves;
        }

        private double GetHorizontalLeg()
        {
            var vertices = _fromCurve.GetVertices();
            vertices.AddRange(_toCurve.GetVertices());
            var closestPoints = vertices.GetClosestPointsAlongVector(_fromCurveVector);
            var horizontalLeg =
                closestPoints.Item1.ToDistanceAlongVector(closestPoints.Item2, _fromCurveVector);
            return horizontalLeg;
        }
    }
}