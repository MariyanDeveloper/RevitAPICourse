using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreRevitLibrary.GeometryUtils
{
    public class AdvancedBoundingBoxXYZ : BoundingBoxXYZ
    {
        public double Length
        {
            get => GetDimensions(Measurement.Length);
            set => SetDimensions(value, Measurement.Length, Alignment.Center);
        }

        public double Width
        {
            get => GetDimensions(Measurement.Width);
            set => SetDimensions(value, Measurement.Width, Alignment.Center);
        }
        public double Height
        {
            get => GetDimensions(Measurement.Height);
            set => SetDimensions(value, Measurement.Height, Alignment.Bottom);
        }

        public XYZ Origin
        {
            get => Transform.Origin;
            set => Move(Origin.ToVector(value));
        }
        public CurveLoop BaseCurveLoop => GetBaseCurveLoop();
        public AdvancedBoundingBoxXYZ(double length = 1, double width = 1, double height = 1)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            Length = length;
            Width = width;
            Height = height;
        }
        public void Align(Measurement measurement, Alignment alignment)
        {
            this.SetDimensions(
                this.GetDimensions(measurement), measurement, alignment);
        }

        public Solid ToSolid(IGeometryShape geometryShape)
        {
            if (geometryShape == null) throw new ArgumentNullException(nameof(geometryShape));
            return geometryShape.ToSolid(this);
        }
        public static AdvancedBoundingBoxXYZ Create(BoundingBoxXYZ boundingBox)
        {
            if (boundingBox == null) throw new ArgumentNullException(nameof(boundingBox));
            AdvancedBoundingBoxXYZ box = new AdvancedBoundingBoxXYZ()
            {
                Min = boundingBox.Min,
                Max = boundingBox.Max,
                Transform = boundingBox.Transform
            };
            return box;
        }

        public void Move(XYZ translation)
        {
            if (translation == null) throw new ArgumentNullException(nameof(translation));
            var transform = Transform;
            var origin = transform.Origin.MoveAlongVector(translation);
            transform.Origin = origin;
            Transform = transform;

        }
        public PlanarFace GetFace(FaceOrientation faceOrientation)
        {
            var faceToNormals = new Dictionary<FaceOrientation, XYZ>()
            {
                {FaceOrientation.Up, this.Transform.BasisZ},
                {FaceOrientation.Down, -this.Transform.BasisZ},
                {FaceOrientation.Left, -this.Transform.BasisX},
                {FaceOrientation.Right, this.Transform.BasisX},
                {FaceOrientation.Front, this.Transform.BasisY},
                {FaceOrientation.Back, -this.Transform.BasisY}
            };
            return this.ToSolid(new Cuboid())
                .Faces.OfType<PlanarFace>()
                .First(x => x.FaceNormal.IsAlmostEqualTo(faceToNormals[faceOrientation]));
        }

        public void Visualize(Document document)
        {
            ToSolid(new Cuboid()).Visualize(document);
            Transform.Visualize(document);
        }


        private double GetDimensions(Measurement measurement)
        {
            var measurementIndex = (int)measurement;
            return this.Max[measurementIndex] - this.Min[measurementIndex];
        }
        private void SetDimensions(double value, Measurement measurement, Alignment alignment)
        {
            var alignmentFactor = (int)alignment;
            var measurementIndex = (int)measurement;
            var minCoordinates = Enumerable.Range(0, 3).Select(x => this.Min[x]).ToList();
            var maxCoordinates = Enumerable.Range(0, 3).Select(x => this.Max[x]).ToList();
            var minValue = -value / 2 * alignmentFactor;
            var maxValue = value + minValue;
            minCoordinates[measurementIndex] = minValue;
            maxCoordinates[measurementIndex] = maxValue;
            this.Min = new XYZ(minCoordinates[0], minCoordinates[1], minCoordinates[2]);
            this.Max = new XYZ(maxCoordinates[0], maxCoordinates[1], maxCoordinates[2]);
        }
        private CurveLoop GetBaseCurveLoop()
        {
            var pt1 = this.Min;
            var pt2 = pt1.MoveAlongVector(XYZ.BasisX * this.Length);
            var pt3 = pt2.MoveAlongVector(XYZ.BasisY * this.Width);
            var pt4 = pt1.MoveAlongVector(XYZ.BasisY * this.Width);
            var curveLoop = CurveLoop.Create(
                new List<Curve>()
                {
                    Line.CreateBound(pt1, pt2).CreateTransformed(this.Transform),
                    Line.CreateBound(pt2, pt3).CreateTransformed(this.Transform),
                    Line.CreateBound(pt3, pt4).CreateTransformed(this.Transform),
                    Line.CreateBound(pt4, pt1).CreateTransformed(this.Transform),
                }
            );
            return curveLoop;
        }


    }
}
