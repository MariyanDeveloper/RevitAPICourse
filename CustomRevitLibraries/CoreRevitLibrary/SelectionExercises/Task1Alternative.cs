using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.TestCommands;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.SelectionExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Task1Alternative : IExternalCommand
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

            var familyInstances = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))
                .Excluding(idsToExclude)
                .WhereElementIsViewIndependent();

            var dataToVisualize = familyInstances.Select(f => $"{f.Category.Name} - {f.ViewSpecific}");
            var testWindow = new TestWindow(dataToVisualize);
            testWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}