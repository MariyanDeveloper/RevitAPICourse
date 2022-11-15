using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.Extensions
{
    public class CurrentDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement, string statusPrompt = "")
        {
            return uiDocument.Selection.PickObjects(
                    ObjectType.Element,
                    new ElementSelectionFilter(validateElement), statusPrompt)
                .Select(r => uiDocument.Document.GetElement(r.ElementId))
                .ToList();
        }
    }
}