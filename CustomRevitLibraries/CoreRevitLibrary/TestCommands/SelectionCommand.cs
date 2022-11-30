using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CoreRevitLibrary.Extensions;
using CoreRevitLibrary.GeometryUtils;
using System.Linq;
using Transform = Autodesk.Revit.DB.Transform;

namespace CoreRevitLibrary.TestCommands
{
    public interface ISectionTransformFactory
    {
        Transform CreateTransform(XYZ facingVector);
    }

    public abstract class BaseSectionTransformFactory : ISectionTransformFactory
    {
        public abstract Transform CreateTransform(XYZ facingVector);

        protected Transform AdaptTransform(Transform transformToAdapt)
        {
            var outputTransform = Transform.Identity;
            outputTransform.BasisZ = transformToAdapt.BasisY;
            outputTransform.BasisY = transformToAdapt.BasisZ;
            outputTransform.BasisX = -transformToAdapt.BasisX;
            return outputTransform;
        }

    }


    public class LongitudinalForwardFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform();
            return AdaptTransform(transform);
        }
    }

    public class LongitudinalBackwardFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }

    public class TransverseRightFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisX.ToTransform();
            return AdaptTransform(transform);
        }
    }

    public class TransverseLeftFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisX.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }

    public class VerticalUpTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisZ.ToTransform();
            return AdaptTransform(transform);
        }
    }
    public class VerticalDownTransformFactory : BaseSectionTransformFactory
    {
        public override Transform CreateTransform(XYZ facingVector)
        {
            var transform = facingVector.ToTransform().BasisZ.Negate().ToTransform();
            return AdaptTransform(transform);
        }
    }

    public class SectionBoundingBoxXYZ : AdvancedBoundingBoxXYZ
    {
        public SectionBoundingBoxXYZ(XYZ facingVector, double locationLength, double height, double farClipOffset, ISectionTransformFactory transformFactory)
         : base(locationLength, height, farClipOffset)
        {
            Transform = transformFactory.CreateTransform(facingVector);
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectionCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var view = document.GetElementByName<ViewSection>("M");
            var cropBox = view.CropBox.ToAdvanced();
            var vector = cropBox.Transform.BasisZ;
            var viewTypeId = document.GetElementsByType<ViewFamilyType>()
                .FirstOrDefault(v => v.ViewFamily == ViewFamily.Section).Id;
            document.Run(() =>
            {
                SectionBoundingBoxXYZ box = new(vector, 7, 4, 5, new LongitudinalForwardFactory());
                ViewSection.CreateSection(
                    document,
                    viewTypeId,
                    box);
            });





            return Result.Succeeded;
        }
    }

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
