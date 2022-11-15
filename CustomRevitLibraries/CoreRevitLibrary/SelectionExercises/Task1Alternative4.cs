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
    public class Task1Alternative4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var categoryIdsToExclude = new List<ElementId>()
            {
                new ElementId(BuiltInCategory.OST_Doors),
                new ElementId(BuiltInCategory.OST_Windows)
            };
            var familyInstances = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance));


            //As you can see the LINQ version is way much better than foreach
            //the main reason is readability 
            var filteredInstances = new List<Element>();
            foreach (var familyInstance in familyInstances)
            {
                if (categoryIdsToExclude.Contains(familyInstance.Category.Id) || familyInstance.ViewSpecific) continue;
                filteredInstances.Add(familyInstance);
            }

            var dataToVisualize = filteredInstances
                .Select(f => $"{f.Category.Name} - {f.ViewSpecific}");
            var testWindow = new TestWindow(dataToVisualize);
            testWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}