using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.Extensions;
using System.Linq;

namespace CoreRevitLibrary.VectorExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ConnectExerciseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var mepCurves = uiDocument
                .PickElements(e => e is MEPCurve, new CurrentDocumentOption())
                .Cast<MEPCurve>().ToList();
            var curves = mepCurves.Select(e => e.GetPlacementCurve())
                .ToList();
            var triangleCalculation = new TriangleCalculation(curves[0], curves[1]);
            var curveConnector = new CurveConnector(triangleCalculation);
            document.Run(() =>
            {
                mepCurves[0].Connect(mepCurves[1], 30.0.ToRadians(), curveConnector);
            });
            return Result.Succeeded;
        }

    }
}