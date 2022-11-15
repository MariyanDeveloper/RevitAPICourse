using Autodesk.Revit.DB;
using System;

namespace CoreRevitLibrary.Extensions
{
    public class LinkableSelectionFilter : BaseSelectionFilter
    {
        private readonly Document _currentDocument;

        public LinkableSelectionFilter(
            Document currentDocument,
            Func<Element, bool> validateElement) : base(validateElement)
        {
            _currentDocument = currentDocument;
        }

        public override bool AllowElement(Element elem)
        {
            return true;
        }

        public override bool AllowReference(Reference reference, XYZ position)
        {
            if (!(_currentDocument.GetElement(reference) is RevitLinkInstance linkInstance))
            {
                return ValidateElement(_currentDocument.GetElement(reference));
            }

            var element = linkInstance.GetLinkDocument()
                .GetElement(reference.LinkedElementId);
            return ValidateElement(element);
        }
    }
}