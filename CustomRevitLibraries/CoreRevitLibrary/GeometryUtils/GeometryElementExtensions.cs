using Autodesk.Revit.DB;
using CoreRevitLibrary.Enums;
using System;
using System.Collections.Generic;

namespace CoreRevitLibrary.GeometryUtils
{
    public static class GeometryElementExtensions
    {
        /// <summary>
        /// This method is used to get root elements of a GeometryElement <see cref="GeometryElement"/> object
        /// </summary>
        /// <typeparam name="T">Desired geometry object</typeparam>
        /// <param name="geometryElement"></param>
        /// <param name="geometryRepresentation">defines whether a geometry is symbol or instance <see cref="GeometryInstance"/></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRootElements<T>(
            this GeometryElement geometryElement,
            GeometryRepresentation geometryRepresentation = GeometryRepresentation.Instance)
            where T : GeometryObject
        {
            if (geometryElement == null) throw new ArgumentNullException(nameof(geometryElement));
            foreach (var geometryObject in geometryElement)
            {
                if (geometryObject is T ultimateElement)
                {
                    yield return ultimateElement;
                }
                else if (geometryObject is GeometryInstance geometryInstance)
                {
                    var familyGeometries = (geometryRepresentation == GeometryRepresentation.Symbol)
                        ? geometryInstance.SymbolGeometry
                        : geometryInstance.GetInstanceGeometry();
                    foreach (var familyGeometry in GetRootElements<T>(familyGeometries, geometryRepresentation))
                    {
                        yield return familyGeometry;
                    }
                }
                else if (geometryObject is GeometryElement nestedGeometryElement)
                {
                    foreach (var nestedElement in GetRootElements<T>(
                                 nestedGeometryElement, geometryRepresentation))
                    {
                        yield return nestedElement;
                    }
                }
            }
        }
    }
}
