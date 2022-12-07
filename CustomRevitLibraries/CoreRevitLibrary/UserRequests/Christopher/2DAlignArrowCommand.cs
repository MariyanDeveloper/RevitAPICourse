using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CoreRevitLibrary.Extensions;
using CoreRevitLibrary.GeometryUtils;

namespace CoreRevitLibrary.UserRequests.Christopher
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AlignArrow2DCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var arrowSymbolFamily = document.GetElementByName<FamilySymbol>("ArrowNonWorkPlane");
            var reference = uiDocument.Selection
                .PickObject(ObjectType.PointOnElement,
                    new ElementSelectionFilter(e => e is MEPCurve, r => r.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_LINEAR)
                    );
            var mepCurve = document.GetElement(reference);
            var levelId = mepCurve.get_Parameter(
                BuiltInParameter.RBS_START_LEVEL_PARAM).AsElementId();
            var mepCurveVector = mepCurve.GetPlacementCurve().ToNormalizedVector();
            var placementPoint = reference.GlobalPoint;
            document.Run(() =>
            {
                var arrowFamily = document.Create.NewFamilyInstance(
                    placementPoint,
                    arrowSymbolFamily,
                    document.GetElement(levelId) as Level,
                    StructuralType.NonStructural);

                #region Align family to placement family
                //do this if the family is below/above the placement point
                document.Regenerate();
                var distance = arrowFamily
                    .GetPlacementPoint()
                    .GetSignedDistance(placementPoint, XYZ.BasisZ);
                arrowFamily.Move(XYZ.BasisZ * distance);
                #endregion

                var rotationResult = mepCurveVector.GetRotationResultTo(XYZ.BasisX);
                var rotationAxis = Line.CreateBound(
                    placementPoint,
                    placementPoint.MoveAlongVector(XYZ.BasisZ));
                ElementTransformUtils.RotateElement(
                    document,
                    arrowFamily.Id,
                    rotationAxis,
                    rotationResult.AroundZ);
            });
            return Result.Succeeded;
        }
    }
}
