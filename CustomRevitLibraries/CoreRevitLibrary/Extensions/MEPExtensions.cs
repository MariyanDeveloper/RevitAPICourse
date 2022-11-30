using Autodesk.Revit.DB;
using CoreRevitLibrary.TestCommands;
using CoreRevitLibrary.VectorExercises;

namespace CoreRevitLibrary.Extensions
{
    public static class MEPExtensions
    {
        public static void Connect(
            this MEPCurve fromMepCurve,
            MEPCurve toMepCurve,
            double angle,
            ICurveConnector curveConnector)
        {
            IConnectionResult connectionResult = curveConnector.Connect(fromMepCurve.GetPlacementCurve(),
                toMepCurve.GetPlacementCurve(), angle);
            fromMepCurve.ChangeCurveLocation(connectionResult.FromCurve);
            fromMepCurve.Clone().ChangeCurveLocation(connectionResult.MiddleCurve);
        }
    }
}
