using System;
using FractalSource.Mapping.Geodesy;

namespace FractalSource.Mapping.Projection
{
    public abstract class MercatorProjection : IEquatable<MercatorProjection>
    {
        /// <summary>
        ///     The typical reference Meridian
        /// </summary>
        public const double GreenwichMeridian = 0.0;

        private Angle _referenceMeridian = GreenwichMeridian;

        /// <summary>
        ///     Instantiate a Mercator projection with this reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe"></param>
        protected MercatorProjection(Ellipsoid referenceGlobe)
        {
            ReferenceGlobe = referenceGlobe;
        }

        /// <summary>
        ///     The reference meridian for the projection, usually this is Greenwich with 0° longitude
        /// </summary>
        public Angle ReferenceMeridian
        {
            get => _referenceMeridian;
            set => _referenceMeridian = NormalizeLongitude(value);
        }

        /// <summary>
        ///     The reference Ellipsoid for this projection
        /// </summary>
        public Ellipsoid ReferenceGlobe { get; private set; }

        /// <summary>
        ///     Get the Mercator scale factor for the given point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The scale factor</returns>
        public abstract double ScaleFactor(GeoCoordinates point);

        /// <summary>
        ///     Convert a latitude/longitude coordinate to a Euclidean coordinate on a flat map
        /// </summary>
        /// <param name="coordinates">The latitude/longitude coordinates in degrees</param>
        /// <returns>The Euclidean coordinates of that point</returns>
        public abstract EuclideanCoordinate ToEuclidean(GeoCoordinates coordinates);

        /// <summary>
        ///     Get the latitude/longitude coordinates from the Euclidean coordinates
        /// </summary>
        /// <param name="xy">The euclidean coordinates</param>
        /// <returns>The latitude/longitude coordinates of that point</returns>
        public abstract GeoCoordinates FromEuclidean(EuclideanCoordinate xy);

        /// <summary>
        ///     Two projections are considered Equal if they are based on
        ///     the same Reference-Globe
        /// </summary>
        /// <param name="other">The projection to compare against</param>
        /// <returns>True if they are equal</returns>
        public bool Equals(MercatorProjection other)
        {
            return ((null != other) && other.ReferenceGlobe.Equals(ReferenceGlobe));
        }


        #region Latitude/Longitude limits

        /// <summary>
        ///     Maximum possible longitude for this projection
        /// </summary>
        public virtual Angle MaxLatitude => Angle.RadiansToDegrees(Math.Atan(Math.Sinh(Math.PI)));

        /// <summary>
        ///     Minimum possible longitude for this projection
        /// </summary>
        public virtual Angle MinLatitude => -MaxLatitude;

        /// <summary>
        ///     Maximum possible longitude for this projection
        /// </summary>
        public static readonly Angle MaxLongitude = 180.0;

        /// <summary>
        ///     Minimum possible longitude for this projection
        /// </summary>
        public static readonly Angle MinLongitude = -180.0;

        /// <summary>
        ///     Ensure Latitude stays in range
        /// </summary>
        /// <param name="latitude">The longitude value</param>
        /// <returns>The normalized longitude</returns>
        public Angle NormalizeLatitude(Angle latitude)
        {
            return Math.Min(MaxLatitude.Degrees, Math.Max(latitude.Degrees, MinLatitude.Degrees));
        }

        /// <summary>
        ///     Ensure Latitude stays in range
        /// </summary>
        /// <param name="longitude">The longitude value</param>
        /// <returns>The normalized longitude</returns>
        public static Angle NormalizeLongitude(Angle longitude)
        {
            return Math.Min(MaxLongitude.Degrees, Math.Max(longitude.Degrees, MinLongitude.Degrees));
        }

        #endregion

        #region Distance calculations

        /// <summary>
        ///     Compute the Euclidean distance between two points given by rectangular coordinates
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        /// <exception cref="ArgumentException">Raised if the two points don't belong to the same projection</exception>
        /// <exception cref="ArgumentNullException">Raised if one of the points is null</exception>
        public double EuclideanDistance(EuclideanCoordinate point1, EuclideanCoordinate point2)
        {
            if (point1 == null || point2 == null)
                throw new ArgumentNullException();
            if (!(point1.Projection.Equals(this) && point2.Projection.Equals(this)))
                throw new ArgumentException("The Euclidean coordinate does not belong to this projection.", nameof(point1));
            return point1.DistanceTo(point2);
        }

        /// <summary>
        ///     Compute the Euclidean distance between two points given by rectangular coordinates.
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        public double EuclideanDistance(GeoCoordinates point1, GeoCoordinates point2)
        {
            return EuclideanDistance(ToEuclidean(point1), ToEuclidean(point2));
        }

        /// <summary>
        ///     Compute the Euclidean distance between two points given by rectangular coordinates
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        /// <exception cref="IndexOutOfRangeException">Raised if one of the arrays is not two-dimensional</exception>
        public double EuclideanDistance(double[] point1, double[] point2)
        {
            return EuclideanDistance(
                new EuclideanCoordinate(this, point1),
                new EuclideanCoordinate(this, point2));
        }

        /// <summary>
        ///     Get the geodesic distance between two points on the globe
        /// </summary>
        /// <param name="start">The starting point</param>
        /// <param name="end">The ending point</param>
        /// <returns>The distance in meters</returns>
        public double GeodesicDistance(GeoCoordinates start, GeoCoordinates end)
        {
            return
                (new GeodeticCalculator(ReferenceGlobe))
                    .CalculateGeodeticCurve(start, end)
                    .EllipsoidalDistance;
        }

        /// <summary>
        ///     Compute the geodesic distance of two points on the globe
        /// </summary>
        /// <param name="longitudeStart">The longitude of the starting point in degrees</param>
        /// <param name="latitudeStart">The longitude of the starting point in degrees</param>
        /// <param name="longitudeEnd">The longitude of the ending point in degrees</param>
        /// <param name="latitudeEnd">The longitude of the ending point in degrees</param>
        /// <returns></returns>
        public double GeodesicDistance(
            double longitudeStart,
            double latitudeStart,
            double longitudeEnd,
            double latitudeEnd)
        {
            var start = new GeoCoordinates(latitudeStart, longitudeStart);
            var end = new GeoCoordinates(latitudeEnd, longitudeEnd);
            return GeodesicDistance(start, end);
        }

        #endregion
    }
}