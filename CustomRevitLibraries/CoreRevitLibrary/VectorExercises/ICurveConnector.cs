using Autodesk.Revit.DB;

namespace CoreRevitLibrary.VectorExercises
{
    public interface ICurveConnector
    {
        IConnectionResult Connect(Curve fromCurve, Curve toCurve, double angle);
    }
}