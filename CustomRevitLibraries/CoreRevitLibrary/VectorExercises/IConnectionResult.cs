using Autodesk.Revit.DB;

namespace CoreRevitLibrary.VectorExercises
{
    public interface IConnectionResult
    {
        Curve FromCurve { get; }
        Curve ToCurve { get; }
        Curve MiddleCurve { get; }
    }
}