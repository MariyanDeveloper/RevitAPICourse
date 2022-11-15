using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace CoreRevitLibrary.Extensions
{
    public interface IPickElementsOption
    {
        List<Element> PickElements(
            UIDocument uiDocument, Func<Element, bool> validateElement, string statusPrompt);
    }
}