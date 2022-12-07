using Autodesk.Revit.DB;
using CoreRevitLibrary.Enums;
using CoreRevitLibrary.GeometryUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.Extensions
{
    public static class ElementExtensions
    {
        /// <summary>
        /// This method is used to get a location curve of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns>curve</returns>
        /// <exception cref="ArgumentNullException">Throws when the object is not curve-driven</exception>
        public static Curve GetPlacementCurve(this Element element)
        {

            if (!(element.Location is LocationCurve locationCurve))
                throw new ArgumentNullException($"The {element.Name} is not curve-driven.");
            return locationCurve.Curve;
        }

        /// <summary>
        /// This method is used to get a location point of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static XYZ GetPlacementPoint(
            this Element element)
        {
            if (!(element.Location is LocationPoint locationPoint))
                throw new ArgumentNullException($"The {element.Name} is not point-driven.");
            return locationPoint.Point;
        }

        public static Element GetElementType(
            this Element element)
        {
            return element.Document.GetElement(element.GetTypeId());
        }

        public static TElement Clone<TElement>(this TElement element)
            where TElement : Element
        {
            var document = element.Document;
            return document.GetElement(ElementTransformUtils
                .CopyElement(
                    document,
                    element.Id,
                    XYZ.Zero).First()) as TElement;
        }

        /// <summary>
        /// This method is used to change location of a curve-driven element
        /// </summary>
        /// <param name="element">Current element</param>
        /// <param name="curve">New location</param>
        /// <exception cref="ArgumentNullException">Element is not curve-driven</exception>
        public static void ChangeCurveLocation(
            this Element element, Curve curve)
        {
            if (!(element.Location is LocationCurve locationCurve))
                throw new ArgumentNullException($"{element.Name} is not curve-driven");
            locationCurve.Curve = curve;
        }

        /// <summary>
        /// This method is used to change location of a point-driven element
        /// </summary>
        /// <param name="element">Current element</param>
        /// <param name="point">New location</param>
        /// <exception cref="ArgumentNullException">Element is not point-driven</exception>
        public static void ChangePointLocation(
            this Element element, XYZ point)
        {
            if (!(element.Location is LocationPoint locationPoint))
                throw new ArgumentNullException($"{element.Name} is not point-driven");
            locationPoint.Point = point;
        }


        public static void Move(this Element element, XYZ translation)
        {
            var document = element.Document;
            ElementTransformUtils.MoveElement(document, element.Id, translation);
        }

        public static void Move(this IEnumerable<Element> elements, XYZ translation)
        {
            var document = elements.First().Document;
            var ids = elements.Select(e => e.Id).ToList();
            ElementTransformUtils.MoveElements(document, ids, translation);
        }

        /// <summary>
        /// This method is used to get any geometry objects from element
        /// </summary>
        /// <typeparam name="T">Geometry object</typeparam>
        /// <param name="element"></param>
        /// <param name="geometryOptions">geometry options</param>
        /// <param name="geometryRepresentation">Symbol or instance geometry</param>
        /// <returns></returns>
        public static IEnumerable<T> GetGeometryObjects<T>(
            this Element element,
            Options geometryOptions = null,
            GeometryRepresentation geometryRepresentation = GeometryRepresentation.Instance)
            where T : GeometryObject
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            geometryOptions ??= new Options();
            var geometryElements = new List<GeometryElement>()
            {
                element.get_Geometry(geometryOptions)
            };
            if (element is FamilyInstance familyInstance)
            {
                if (familyInstance.GetSubComponentIds().Any())
                {
                    geometryElements.AddRange(familyInstance.GetSubComponentIds()
                        .Select(subComponentId => element.Document.GetElement(subComponentId))
                        .Select(subComponent => subComponent.get_Geometry(geometryOptions)));
                }
            }
            if (geometryElements.Any(x => x == null))
                return Enumerable.Empty<T>();

            return geometryElements.SelectMany(x => x.GetRootElements<T>(
                geometryRepresentation));

        }


    }
}
