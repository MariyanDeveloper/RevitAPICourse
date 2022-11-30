using Autodesk.Revit.DB;
using System;

namespace CoreRevitLibrary.VectorExercises
{
    public class ConnectionResult : IConnectionResult
    {
        public Curve FromCurve { get; }
        public Curve ToCurve { get; }
        public Curve MiddleCurve { get; }

        public ConnectionResult(Curve fromCurve, Curve toCurve, Curve middleCurve)
        {
            FromCurve = fromCurve ?? throw new ArgumentNullException(nameof(fromCurve));
            ToCurve = toCurve ?? throw new ArgumentNullException(nameof(toCurve));
            MiddleCurve = middleCurve ?? throw new ArgumentNullException(nameof(middleCurve));
        }
    }
}