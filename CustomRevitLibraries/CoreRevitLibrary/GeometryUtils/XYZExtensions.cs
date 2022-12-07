using Autodesk.Revit.DB;
using CoreRevitLibrary.Enums;
using CoreRevitLibrary.Extensions;
using System;
using System.Collections.Generic;

namespace CoreRevitLibrary.GeometryUtils
{
    public static class XYZExtensions
    {
        /// <summary>
        /// This method is used to get a curve from vector
        /// </summary>
        /// <param name="vector">Given vector</param>
        /// <param name="origin">Origin</param>
        /// <param name="length">Length</param>
        /// <returns></returns>
        public static Curve AsCurve(
            this XYZ vector, XYZ origin = null, double? length = null)
        {
            origin ??= XYZ.Zero;
            length ??= vector.GetLength();
            return Line.CreateBound(
                origin,
                origin.MoveAlongVector(vector, length.GetValueOrDefault()));
        }

        /// <summary>
        /// This method is used to move point along a given vector
        /// </summary>
        /// <param name="pointToMove">Point to move</param>
        /// <param name="vector">Given vector</param>
        /// <returns>Moved point</returns>
        public static XYZ MoveAlongVector(
            this XYZ pointToMove, XYZ vector) => pointToMove.Add(vector);

        /// <summary>
        /// This method is used to move point along a given vector and distance
        /// </summary>
        /// <param name="pointToMove">Point to move</param>
        /// <param name="vector">Given vector</param>
        /// <param name="distance">Given distance</param>
        /// <returns></returns>
        public static XYZ MoveAlongVector(
            this XYZ pointToMove, XYZ vector, double distance) => pointToMove.Add(vector.Normalize() * distance);



        /// <summary>
        /// This method is used to visualize XYZ in a document
        /// </summary>
        /// <param name="point"></param>
        /// <param name="document"></param>
        public static void Visualize(
            this XYZ point, Document document)
        {
            document.CreateDirectShape(Point.Create(point));
        }

        /// <summary>
        /// Get a vector from one point to another
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static XYZ ToVector(
            this XYZ firstPoint, XYZ secondPoint)
        {
            return (secondPoint - firstPoint);
        }

        /// <summary>
        /// Get a normalized vector from one point to another
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static XYZ ToNormalizedVector(
            this XYZ firstPoint, XYZ secondPoint)
        {
            return (secondPoint - firstPoint).Normalize();
        }

        /// <summary>
        /// This method is used to get a distance b/w 2 points along a given vector
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="vector">normalized vector to measure by</param>
        /// <returns></returns>
        public static double ToDistanceAlongVector(
            this XYZ firstPoint, XYZ secondPoint, XYZ vector)
        {
            return Math.Abs(
                firstPoint.ToVector(secondPoint).DotProduct(vector));
        }

        /// <summary>
        /// This method is used to get a signed distance
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double GetSignedDistance(
            this XYZ firstPoint, XYZ secondPoint, XYZ vector)
        {
            return firstPoint.ToVector(secondPoint).DotProduct(vector);
        }

        public static XYZ ProjectOntoPlane(
            this XYZ pointToProject, Plane plane)
        {
            var distance = plane.Origin.GetSignedDistance(
                pointToProject, plane.Normal);
            var projectedPoint = pointToProject - distance * plane.Normal;
            return projectedPoint;
        }


        public static Tuple<XYZ, XYZ> GetClosestPoints(this IList<XYZ> points)
        {
            Tuple<XYZ, XYZ> result = default;
            var distance = double.NaN;
            for (var i = 0; i < points.Count; i++)
            {
                var pt1 = points[i];
                for (var j = 0; j < points.Count; j++)
                {
                    if (i == j) continue;
                    var pt2 = points[j];
                    var currentDistance = pt1.DistanceTo(pt2);
                    if (double.IsNaN(distance) || currentDistance < distance)
                    {
                        result = new Tuple<XYZ, XYZ>(pt1, pt2);
                        distance = currentDistance;
                    }
                }
            }
            return result;
        }

        public static Tuple<XYZ, XYZ> GetClosestPointsAlongVector(this IList<XYZ> points, XYZ vector)
        {
            Tuple<XYZ, XYZ> result = default;
            var distance = double.NaN;
            for (var i = 0; i < points.Count; i++)
            {
                var pt1 = points[i];
                for (var j = 0; j < points.Count; j++)
                {
                    if (i == j) continue;
                    var pt2 = points[j];
                    var currentDistance = pt1.ToDistanceAlongVector(pt2, vector);
                    if (double.IsNaN(distance) || currentDistance < distance)
                    {
                        result = new Tuple<XYZ, XYZ>(pt1, pt2);
                        distance = currentDistance;
                    }
                }
            }
            return result;
        }

        public static Tuple<XYZ, XYZ> GetFurthestPoints(this IList<XYZ> points)
        {
            Tuple<XYZ, XYZ> result = default;
            var distance = double.NaN;
            for (var i = 0; i < points.Count; i++)
            {
                var pt1 = points[i];
                for (var j = 0; j < points.Count; j++)
                {
                    if (i == j)
                        continue;
                    var pt2 = points[j];
                    var currentDistance = pt1.DistanceTo(pt2);
                    if (double.IsNaN(distance) || currentDistance > distance)
                    {
                        result = new Tuple<XYZ, XYZ>(pt1, pt2);
                        distance = currentDistance;
                    }
                }
            }
            return result;
        }


        public static Tuple<XYZ, XYZ> GetFurthestPointsAlongVector(this IList<XYZ> points, XYZ vector)
        {
            Tuple<XYZ, XYZ> result = default;
            var distance = double.NaN;
            for (var i = 0; i < points.Count; i++)
            {
                var pt1 = points[i];
                for (var j = 0; j < points.Count; j++)
                {
                    if (i == j)
                        continue;
                    var pt2 = points[j];
                    var currentDistance = pt1.ToDistanceAlongVector(pt2, vector);
                    if (double.IsNaN(distance) || currentDistance > distance)
                    {
                        result = new Tuple<XYZ, XYZ>(pt1, pt2);
                        distance = currentDistance;
                    }
                }
            }
            return result;
        }


        public static IRotationResult GetRotationResultTo(
            this XYZ fromVector, XYZ toVector)
        {
            var aroundX = fromVector.AngleOnPlaneTo(toVector, XYZ.BasisX);
            var aroundY = fromVector.AngleOnPlaneTo(toVector, XYZ.BasisY);
            var aroundZ = fromVector.AngleOnPlaneTo(toVector, XYZ.BasisZ);
            return new RotationResult(-aroundX, -aroundY, -aroundZ);
        }

        public static Transform ToTransform(this XYZ vector, Orientation orientation = Orientation.Facing)
        {
            var comparisonVector = XYZ.BasisY;
            var rotation = vector.GetRotationResultTo(comparisonVector);
            var transform = Transform.CreateRotation(XYZ.BasisZ, rotation.AroundZ);
            var normal = transform.BasisX;
            var angle = transform.BasisY.AngleOnPlaneTo(vector, normal);
            var outputTransform = transform.Multiply(Transform.CreateRotation(XYZ.BasisX, angle));
            if (orientation == Orientation.Facing) return outputTransform;
            return outputTransform.Multiply(Transform.CreateRotation(XYZ.BasisZ, Math.PI / 2));
        }

        public static VectorRelation GetRelationTo(
            this XYZ fromVector, XYZ toVector)
        {
            if (fromVector.DotProduct(toVector).IsAlmostEqualTo(1))
            {
                return VectorRelation.Equal;
            }
            if (fromVector.DotProduct(toVector).IsAlmostEqualTo(-1))
            {
                return VectorRelation.Reversed;
            }
            if (fromVector.DotProduct(toVector).IsAlmostEqualTo(0))
            {
                return VectorRelation.Perpendicular;
            }
            return VectorRelation.Undefined;
        }

    }
}
