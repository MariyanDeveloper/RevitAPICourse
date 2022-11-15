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
    public class Task4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var ids = new List<int>() { 207592, 15915 };
            var viewIds = ids
                .Select(id => new ElementId(id))
                .ToList();
            var components = viewIds
                .SelectMany(id => new FilteredElementCollector(document, id).OfClass(typeof(FamilyInstance)).OwnedByView(id))
                .ToList();


            var answer = components.Max(e => e.Id.IntegerValue);
            TaskDialog.Show("Message", $"answer is {answer}");
            TestWindow testWindow = new TestWindow(components.Select(c => c.Id));
            testWindow.ShowDialog();
            return Result.Succeeded;

        }
    }
}