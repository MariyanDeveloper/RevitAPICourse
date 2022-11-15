using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.Extensions
{
    /// <summary>
    /// This class is used for adding extension methods for UIDocument class, <see cref="Autodesk.Revit.UI.UIDocument"/>
    /// </summary>
    public static class UIDocumentExtensions
    {
        public static List<Element> GetSelectedElements(this UIDocument uiDocument)
        {
            return uiDocument.Selection
                .GetElementIds()
                .Select(id => uiDocument.Document.GetElement(id))
                .ToList();
        }

        public static List<Element> PickElements(
            this UIDocument uiDocument,
            Func<Element, bool> validateElement,
            IPickElementsOption pickElementsOption,
            string statusPrompt = "")
        {
            return pickElementsOption.PickElements(uiDocument, validateElement, statusPrompt);
        }
    }

}
