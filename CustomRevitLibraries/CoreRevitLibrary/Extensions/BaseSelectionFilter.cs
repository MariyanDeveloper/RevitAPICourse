using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace CoreRevitLibrary.Extensions
{
    public abstract class BaseSelectionFilter : ISelectionFilter
    {
        protected Func<Element, bool> ValidateElement { get; }

        protected BaseSelectionFilter(Func<Element, bool> validateElement)
        {
            ValidateElement = validateElement;
        }

        public abstract bool AllowElement(Element elem);

        public abstract bool AllowReference(Reference reference, XYZ position);

    }
}