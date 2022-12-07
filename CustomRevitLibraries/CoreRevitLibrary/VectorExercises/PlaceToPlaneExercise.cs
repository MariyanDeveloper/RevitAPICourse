using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CoreRevitLibrary.Extensions;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.VectorExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class PlaceToPlaneExercise : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var familyToMove = uiDocument.GetSelectedElements()[0];
            var point = uiDocument.Selection.PickObject(ObjectType.PointOnElement).GlobalPoint;
            var normal = XYZ.BasisZ;
            var elevation = XYZ.Zero
                .GetSignedDistance(point, normal);
            document.Run(() =>
            {
                var level = Level.Create(document, elevation);
                familyToMove.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM).Set(level.Id);
            });
            return Result.Succeeded;
        }
    }
}