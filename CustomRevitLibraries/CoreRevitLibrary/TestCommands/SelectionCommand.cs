using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.Extensions;

namespace CoreRevitLibrary.TestCommands
{
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
            var selectedWall = uiDocument.GetSelectedElements()[0];
            var secondLevel = document.GetElementByName<Level>("Level 2");
            var baseConstraintParameter = selectedWall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT);
            using (Transaction transaction = new Transaction(document, "Set new mark"))
            {
                TaskDialog.Show("Before", transaction.GetStatus().ToString());
                transaction.Start();
                TaskDialog.Show("Inside", transaction.GetStatus().ToString());
                baseConstraintParameter.Set(secondLevel.Id);
                transaction.Commit();
                TaskDialog.Show("After", transaction.GetStatus().ToString());
            }

            //var window = new TestWindow(new List<string>() { level.Name });
            //window.ShowDialog();
            return Result.Succeeded;
        }



    }
}
