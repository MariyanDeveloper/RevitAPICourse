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
    public class Task3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            ParameterValueProvider markValueProvider =
                new ParameterValueProvider(new ElementId(BuiltInParameter.DOOR_NUMBER));
            FilterStringEquals filterStringContains = new FilterStringEquals();
            FilterStringRule filterStringRule = new FilterStringRule(
                markValueProvider,
                filterStringContains,
                "revit",
                false);
            ElementParameterFilter parameterFilter = new ElementParameterFilter(filterStringRule);
            var types = new List<Type>()
            {
                typeof(Wall),
                typeof(FamilyInstance)
            };
            ElementMulticlassFilter multiclassFilter = new ElementMulticlassFilter(types);
            var collector = new FilteredElementCollector(document)
                .WherePasses(multiclassFilter)
                .WherePasses(parameterFilter);
            var answer = collector.GetElementCount();
            TaskDialog.Show("Message", $"The answer is : {answer}");

            //if you want to see the value of elements from your collector
            var parameterValues = collector
                .Select(e => e.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString())
                .ToList();
            TestWindow testWindow = new TestWindow(parameterValues);
            testWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
