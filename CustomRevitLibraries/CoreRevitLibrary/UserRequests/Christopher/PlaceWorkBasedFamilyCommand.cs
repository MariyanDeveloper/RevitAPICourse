using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CoreRevitLibrary.Extensions;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.UserRequests.Christopher
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class PlaceWorkBasedFamilyCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var arrowSymbolFamily = document.GetElementByName<FamilySymbol>("ArrowWorkPlane");
            var reference = uiDocument.Selection
                .PickObject(ObjectType.PointOnElement,
                    new ElementSelectionFilter(e => e is MEPCurve, r => r.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_LINEAR)
                );
            var mepCurve = document.GetElement(reference);
            var normal = mepCurve.GetPlacementCurve().ToNormalizedVector();
            var placementPoint = reference.GlobalPoint;
            var plane = Plane.CreateByNormalAndOrigin(
                normal,
                placementPoint);
            document.Run(() =>
            {
                var referencePlane = document.Create.CreateReferencePlane(
                    plane,
                    document.ActiveView,
                    "ArrowPlacementPlane1");
                document.Create.NewFamilyInstance(
                    referencePlane.GetReference(),
                    placementPoint,
                    plane.XVec,
                    arrowSymbolFamily);

            });


            return Result.Succeeded;
        }
    }
}