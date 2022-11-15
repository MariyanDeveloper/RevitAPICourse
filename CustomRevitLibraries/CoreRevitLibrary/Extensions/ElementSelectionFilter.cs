using Autodesk.Revit.DB;
using System;

namespace CoreRevitLibrary.Extensions
{
    public class ElementSelectionFilter : BaseSelectionFilter
    {
        private readonly Func<Reference, bool> _validateReference;

        public ElementSelectionFilter(
            Func<Element, bool> validateElement) : base(validateElement)
        {
        }

        public ElementSelectionFilter(Func<Element, bool> validateElement,
            Func<Reference, bool> validateReference)
            : base(validateElement)
        {
            _validateReference = validateReference;
        }
        public override bool AllowElement(Element elem)
        {

            return ValidateElement(elem);
        }

        public override bool AllowReference(Reference reference, XYZ position)
        {
            if (_validateReference == null) return true;
            return _validateReference.Invoke(reference);

        }


    }
}