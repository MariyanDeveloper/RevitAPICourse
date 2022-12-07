using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

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


            return Result.Succeeded;
        }
    }
}
