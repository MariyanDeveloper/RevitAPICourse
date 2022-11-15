using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.Extensions
{
    public class LinkDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement, string statusPrompt = "")
        {
            var document = uiDocument.Document;
            var references = uiDocument.Selection.PickObjects(
                ObjectType.LinkedElement,
                new LinkableSelectionFilter(document, validateElement), statusPrompt);
            var elements = references
                .Select(r => (document.GetElement(r.ElementId) as RevitLinkInstance)
                    .GetLinkDocument().GetElement(r.LinkedElementId))
                .ToList();
            return elements;

        }
    }
}