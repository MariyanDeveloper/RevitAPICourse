using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.TestCommands;

namespace CoreRevitLibrary.SelectionExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Task1Alternative2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var categoriesToExclude = new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_Doors,
                BuiltInCategory.OST_Windows
            };
            ElementMulticategoryFilter multicategoryFilter = new ElementMulticategoryFilter(categoriesToExclude);

            var idsToExclude = new FilteredElementCollector(document)
                .WherePasses(multicategoryFilter)
                .WhereElementIsNotElementType()
                .ToElementIds();
            ExclusionFilter exclusionFilter = new ExclusionFilter(idsToExclude);

            LogicalAndFilter logicalAndFilter = new LogicalAndFilter(
                new ElementClassFilter(typeof(FamilyInstance)),
                exclusionFilter);

            var familyInstances = new FilteredElementCollector(document)
                .WherePasses(logicalAndFilter)
                .WhereElementIsViewIndependent();

            var dataToVisualize = familyInstances.Select(f => $"{f.Category.Name} - {f.ViewSpecific}");
            var testWindow = new TestWindow(dataToVisualize);
            testWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}