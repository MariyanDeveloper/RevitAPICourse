using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.TestCommands;
using System.Linq;

namespace CoreRevitLibrary.SelectionExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Task2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            ElementId activeViewId = document.ActiveView.Id;
            var collector = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))
                .OwnedByView(activeViewId);
            var answer = collector.Sum(e => e.Id.IntegerValue);
            TaskDialog.Show("Message", $"The answer is : {answer}");

            var dataToVisualize = collector.Select(e => $"{e.Name} - {e.ViewSpecific}");
            var testWindow = new TestWindow(dataToVisualize);
            testWindow.ShowDialog();


            return Result.Succeeded;
        }
    }
}
