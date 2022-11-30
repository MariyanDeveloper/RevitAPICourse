using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.Extensions;
using System;
using System.Linq;

namespace CoreRevitLibrary.ParametersExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ParameterExercise2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var firstLevel = document.GetElementByName<Level>("Level 1");
            var wallTypesToExclude = document.GetElementsByType<Wall>()
                .Where(w => !w.IsCurtain())
                .Where(w =>
                    w.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId() == firstLevel.Id &&
                    w.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble() > 1 &&
                    w.WallType.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString() == "TheOne")
                .Select(w => w.WallType)
                .ToList();

            var wallTypeIds = document.GetElementsByType<WallType>()
                .Where(wt => wallTypesToExclude.Select(w => w.Id).All(id => wt.Id != id))
                .Select(w => w.Id)
                .ToList();

            var walls = document.GetElementsByType<Wall>()
                .Where(w => wallTypeIds.Any(wt => w.WallType.Id == wt))
                .ToList();
            var levelTypeToSet = document.GetElementByName<LevelType>("CustomLevelType");
            var baseLevelIds = walls
                .Select(w => w.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId())
                .Distinct();
            var sum = 0.0;
            document.Run(() =>
            {
                foreach (var baseLevelId in baseLevelIds)
                {
                    var level = document.GetElement(baseLevelId);
                    level.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(levelTypeToSet.Id);
                    var levelElevation = level.get_Parameter(BuiltInParameter.LEVEL_ELEV);
                    var elevationBefore = levelElevation.AsDouble();
                    levelElevation.Set(elevationBefore + 1.5);
                    var elevationAfter = levelElevation.AsDouble();
                    sum += elevationBefore + elevationAfter;
                    //document.Regenerate();
                    //TaskDialog.Show("Message", $"Before {elevationBefore}, After {elevationAfter}");
                }
            });
            var answer = Math.Round(sum, 2);
            TaskDialog.Show("Answer", answer.ToString());
            return Result.Succeeded;
        }
    }
}