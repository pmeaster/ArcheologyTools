using System;
using System.Collections.Generic;
using FractalSource.Mapping.Geodesy;

namespace FractalSource.Mapping.Projection
{
    public class EuclideanCoordinate : IEquatable<EuclideanCoordinate>
    {
        /// <summary>
        ///     The X coordinate
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     The Y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        ///     Instantiate a new point
        /// </summary>
        /// <param name="projection">The projection owning these coordinates</param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public EuclideanCoordinate(MercatorProjection projection, double x, double y)
        {
            Projection = projection;
            X = x;
            Y = y;
        }

        /// <summary>
        ///     The default coordinates (X and Y are zero)
        /// </summary>
        /// <param name="projection">The projection owning these coordinates</param>
        public EuclideanCoordinate(MercatorProjection projection) : this(projection, 0.0, 0.0)
        { }

        /// <summary>
        ///     Instantiate a new point from a coordinate array
        /// </summary>
        /// <param name="projection">The projection owning these coordinates</param>
        /// <param name="xy">List of xy coordinates</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the array is not two-dimensional</exception>
        public EuclideanCoordinate(MercatorProjection projection, IReadOnlyList<double> xy)
        {
            if (xy.Count != 2)
                throw new IndexOutOfRangeException("A coordinate array must have exactly two elements.");
            Projection = projection;
            X = xy[0];
            Y = xy[1];
        }

        /// <summary>
        ///     The Mercator projection that owns these coordinates
        /// </summary>
        public MercatorProjection Projection { get; }

        /// <summary>
        ///     Check whether another Euclidean point belongs to the same projection
        /// </summary>
        /// <param name="other">The other point</param>
        /// <returns>True if they belong to the same projection, false otherwise</returns>
        protected virtual bool IsSameProjection(EuclideanCoordinate other)
        {
            return (other != null && other.Projection.Equals(Projection));
        }

        /// <summary>
        ///     Compute the Euclidean distance to another point
        /// </summary>
        /// <param name="other">The other point</param>
        /// <returns>The distance</returns>
        /// <exception cref="ArgumentException">Raised if the two points don't belong to the same projection</exception>
        public virtual double DistanceTo(EuclideanCoordinate other)
        {
            if (!IsSameProjection(other))
                throw new ArgumentException("The Euclidean coordinates do not belong to the same projection.");
            return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
        }

        /// <summary>
        ///     Check whether another coordinate is close to this one in a given precision
        /// </summary>
        /// <param name="other">The other coordinate</param>
        /// <param name="precision">The precision (defaults to some small value)</param>
        /// <returns>True if the coordinates are nearly the same.</returns>
        protected virtual bool IsApproximatelyEqual(
            EuclideanCoordinate other,
            double precision = Constants.DefaultPrecision)
        {
            if (!IsSameProjection(other))
                return false;
            return (other != null && other.Projection.Equals(Projection) &&
                    other.X.IsApproximatelyEqualTo(X, precision) &&
                    other.Y.IsApproximatelyEqualTo(Y, precision));
        }

        /// <summary>
        ///     Test another coordinate to be the same coordinates.
        /// </summary>
        /// <param name="other">The coordinates to test</param>
        /// <returns>True if this is the same coordinate</returns>
        public bool Equals(EuclideanCoordinate other)
        {
            return other != null && (IsApproximatelyEqual(other));
        }

        /// <summary>
        ///     Test another object to be the same coordinates.
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <returns>True if the object is the same coordinate</returns>
        public override bool Equals(object obj)
        {
            var other = obj as EuclideanCoordinate;
            return ((IEquatable<EuclideanCoordinate>)this).Equals(other);
        }

        /// <summary>
        ///     The Hashcode of the coordinates
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            double[] xy = { X, Y, Projection.ReferenceGlobe.SemiMajorAxis, Projection.ReferenceGlobe.Flattening };
            return xy.GetHashCode();
        }

    }
}