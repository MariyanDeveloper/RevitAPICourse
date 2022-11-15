using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CoreRevitLibrary.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.SelectionExercises
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Task3Alternative : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            var types = new List<Type>()
            {
                typeof(Wall),
                typeof(FamilyInstance)
            };

            ElementMulticlassFilter multiclassFilter = new ElementMulticlassFilter(types);
            var outputElements = new FilteredElementCollector(document)
                .WherePasses(multiclassFilter)
                .Where(e => string.Equals(
                    e.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString(),
                    "revit",
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
            var answer = outputElements.Count;
            TaskDialog.Show("Message", $"The answer is : {answer}");

            //if you want to see the value of elements from your collector
            var parameterValues = outputElements
                .Select(e => e.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString())
                .ToList();
            TestWindow testWindow = new TestWindow(parameterValues);
            testWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}
