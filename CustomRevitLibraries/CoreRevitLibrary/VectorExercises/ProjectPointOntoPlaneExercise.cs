using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.Extensions;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.VectorExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ProjectPointOntoPlaneExercise : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var family = uiDocument.GetSelectedElements()[0];
            var normal = new XYZ(1, 1, 0);
            var origin = new XYZ(32, 0, 0);
            var plane = Plane.CreateByNormalAndOrigin(normal, origin);

            document.Run(() =>
            {
                family.GetPlacementPoint().ProjectOntoPlane(plane).Visualize(document);
                plane.Visualize(document);

            });
            return Result.Succeeded;
        }
    }
}