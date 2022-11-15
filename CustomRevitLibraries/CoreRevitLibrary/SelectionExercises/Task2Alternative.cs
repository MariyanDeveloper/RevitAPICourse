using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.TestCommands;

namespace CoreRevitLibrary.SelectionExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Task2Alternative : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            ElementId activeViewId = document.ActiveView.Id;
            ElementOwnerViewFilter ownerViewFilter = new ElementOwnerViewFilter(activeViewId);
            var collector = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))
                .WherePasses(ownerViewFilter);
            var answer = collector.Sum(e => e.Id.IntegerValue);
            TaskDialog.Show("Message", $"The answer is : {answer}");

            var dataToVisualize = collector.Select(e => $"{e.Name} - {e.ViewSpecific}");
            var testWindow = new TestWindow(dataToVisualize);
            testWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}