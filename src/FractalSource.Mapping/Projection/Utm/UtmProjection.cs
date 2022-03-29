using System;
using FractalSource.Mapping.Geodesy;

namespace FractalSource.Mapping.Projection.Utm
{
    /// <summary>
    ///     The Universal Transverse Mercator Projection
    /// </summary>
    public class UtmProjection : MercatorProjection
    {
        internal const double SouthernNorthernOffset = 10000000.0;

        /// <summary>
        ///     Instantiate an UTM projection with WGS84 as reference
        ///     ///
        /// </summary>
        public UtmProjection()
            : this(Ellipsoid.Wgs84)
        {
        }

        /// <summary>
        ///     Instantiate an UTM projection with this reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe">The reference Ellipsoid</param>
        public UtmProjection(Ellipsoid referenceGlobe)
            : base(referenceGlobe)
        {
            _m = new MathConstants(referenceGlobe);
        }

        /// <summary>
        ///     Minimum latitude mapped by the UTM projection
        /// </summary>
        public override Angle MinLatitude => -80.0;

        /// <summary>
        ///     Maximum latitude mapped by the UTM projection
        /// </summary>
        public override Angle MaxLatitude => 84.0;

        private EuclideanCoordinate ToUtmCoordinates(
            GeoCoordinates coordinates,
            out double scaleFactor,
            out double meridianConvergence)
        {
            var grid = new UtmGrid(this, coordinates);

            var northernOffset = grid.IsNorthern ? 0.0 : SouthernNorthernOffset;

            // Various constants for the mathematical approximations

            var phi = coordinates.Latitude.Radians;
            var lambda = coordinates.Longitude.Radians;
            var lambda0 = grid.CenterMeridian.Radians;

            var x = 2.0 * Math.Sqrt(_m.N) / (1.0 + _m.N);
            var t = Math.Sinh(ATanH(Math.Sin(phi)) - ATanH(x * Math.Sin(phi)) * x);
            var chiTick = Math.Atan(t / Math.Cos(lambda - lambda0));
            var etaTick = ATanH(Math.Sin(lambda - lambda0) / Math.Sqrt(1.0 + t * t));

            var sigma = 1.0;
            for (var j = 1; j <= 3; j++)
            {
                sigma += (2.0 * j * _m.Alpha[j - 1] * Math.Cos(2.0 * j * chiTick) * Math.Cosh(2.0 * j * etaTick));
            }

            var tau = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                tau += (2.0 * j * _m.Alpha[j - 1] * Math.Sin(2.0 * j * chiTick) * Math.Sinh(2.0 * j * etaTick));
            }

            var sum0 = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                sum0 += (_m.Alpha[j - 1] * Math.Cos(2.0 * j * chiTick) * Math.Sinh(2.0 * j * etaTick));
            }
            var easting = MathConstants.E0 + MathConstants.K0 * _m.A * (etaTick + sum0);

            sum0 = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                sum0 += (_m.Alpha[j - 1] * Math.Sin(2.0 * j * chiTick) * Math.Cosh(2.0 * j * etaTick));
            }
            var northern = northernOffset + MathConstants.K0 * _m.A * (chiTick + sum0);

            scaleFactor = (MathConstants.K0 * _m.A / ReferenceGlobe.SemiMajorAxis) *
                          Math.Sqrt(((sigma * sigma + tau * tau) / (t * t + Math.Pow(Math.Cos(lambda - lambda0), 2.0))) *
                                    (1.0 + Math.Pow(Math.Tan(phi) * ((1.0 - _m.N) / (1.0 + _m.N)), 2.0)));
            meridianConvergence = Math.Atan((tau * Math.Sqrt(1.0 + t * t) + sigma * t * Math.Tan(lambda - lambda0)) /
                                            (sigma * Math.Sqrt(1.0 + t * t) - tau * t * Math.Tan(lambda - lambda0)));

            return new UtmCoordinate(grid, easting, northern, scaleFactor, meridianConvergence);
        }

        /// <summary>
        ///     Convert a latitude/longitude coordinate to a Euclidean coordinate on a flat map
        /// </summary>
        /// <param name="coordinates">The latitude/longitude coordinates in degrees</param>
        /// <returns>The Euclidean coordinates of that point</returns>
        public override EuclideanCoordinate ToEuclidean(GeoCoordinates coordinates)
        {
            return ToUtmCoordinates(coordinates, out _, out _);
        }

        internal GeoCoordinates FromEuclidean(EuclideanCoordinate xy, out double scaleFactor, out double meridianConvergence)
        {
            if (xy is not UtmCoordinate point)
            {
                throw new ArgumentException("The coordinate must be an _UtmCoordinate.");
            }

            var hemisphere = point.Grid.IsNorthern ? 1 : -1;

            var northernOffset = point.Grid.IsNorthern ? 0.0 : SouthernNorthernOffset;
            var chi = (point.Y - northernOffset) / (MathConstants.K0 * _m.A);
            var eta = (point.X - MathConstants.E0) / (MathConstants.K0 * _m.A);

            var sum = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                sum += (_m.Beta[j - 1] * Math.Sin(2.0 * j * chi) * Math.Cosh(2.0 * j * eta));
            }
            var chiTick = chi - sum;

            sum = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                sum += (_m.Beta[j - 1] * Math.Cos(2.0 * j * chi) * Math.Sinh(2.0 * j * eta));
            }
            var etaTick = eta - sum;

            sum = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                sum += (2.0 * j * _m.Beta[j - 1] * Math.Cos(2.0 * j * chi) * Math.Cosh(2.0 * j * eta));
            }
            var sigmaTick = 1.0 - sum;

            var tauTick = 0.0;
            for (var j = 1; j <= 3; j++)
            {
                tauTick += (2.0 * j * _m.Beta[j - 1] * Math.Sin(2.0 * j * chi) * Math.Sinh(2.0 * j * eta));
            }
            var xi = Math.Asin(Math.Sin(chiTick) / Math.Cosh(etaTick));

            var phi = xi;
            for (var j = 1; j <= 3; j++)
            {
                phi += (_m.Delta[j - 1] * Math.Sin(2.0 * j * xi));
            }

            var lambda0 = point.Grid.CenterMeridian.Radians;
            var lambda = lambda0 + Math.Atan(Math.Sinh(etaTick) / Math.Cos(chiTick));
            var k = ((MathConstants.K0 * _m.A) / ReferenceGlobe.SemiMajorAxis) *
                    Math.Sqrt(((Math.Pow(Math.Cos(chiTick), 2.0) + Math.Pow(Math.Sinh(etaTick), 2.0)) /
                               (sigmaTick * sigmaTick + tauTick * tauTick)) *
                              (1.0 + Math.Pow(((1.0 - _m.N) / (1.0 + _m.N)) * Math.Tan(phi), 2.0)));
            var gamma = Math.Atan((tauTick + sigmaTick * Math.Tan(chiTick) * Math.Tanh(etaTick)) /
                                  (sigmaTick - tauTick * Math.Tan(chiTick) * Math.Tanh(etaTick))) * hemisphere;

            scaleFactor = k;
            meridianConvergence = gamma;
            return new GeoCoordinates(Angle.RadiansToDegrees(phi), Angle.RadiansToDegrees(lambda));
        }

        /// <summary>
        ///     Get the latitude/longitude coordinates from the Euclidean coordinates
        /// </summary>
        /// <param name="xy">The Euclidean coordinates</param>
        /// <returns>The latitude/longitude coordinates of that point</returns>
        public override GeoCoordinates FromEuclidean(EuclideanCoordinate xy)
        {
            return FromEuclidean(xy, out _, out _);
        }

        /// <summary>
        ///     Get the Mercator scale factor for the given point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The scale factor</returns>
        public override double ScaleFactor(GeoCoordinates point)
        {
            ToUtmCoordinates(point, out var scaleFactor, out _);
            return scaleFactor;
        }

        /// <summary>
        ///     Compute the meridian convergence for a location
        /// </summary>
        /// <param name="point">The location defined by latitude/longitude</param>
        /// <returns>The meridian convergence</returns>
        public Angle MeridianConvergence(GeoCoordinates point)
        {
            ToUtmCoordinates(point, out _, out var meridianConvergence);
            return Angle.RadiansToDegrees(meridianConvergence);
        }

        /*
         * This implementation is based on the formulas discussed in
         * http://en.wikipedia.org/wiki/Universal_Transverse_Mercator_coordinate_system
         * 
         * */
        private class MathConstants
        {
            public readonly double[] Alpha, Beta, Delta;
            public const double E0 = 500000;
            public const double K0 = 0.9996;
            public readonly double N, A;

            internal MathConstants(Ellipsoid ellipsoid)
            {
                N = ellipsoid.Flattening / (2.0 - ellipsoid.Flattening);
                var n2 = N * N;
                var n3 = n2 * N;
                var n4 = n3 * N;
                A = (ellipsoid.SemiMajorAxis / (1.0 + N)) * (1.0 + n2 / 4.0 + n4 / 64.0);
                Alpha = new[]
                {
                    N*0.5 - 2.0*n2/3.0 + 5.0*n3/16.0,
                    13.0*n2/48.0 - 0.6*n3,
                    61.0*n3/240.0
                };
                Beta = new[]
                {
                    N*0.5 - 2.0*n2/3.0 + 37.0*n3/96.0,
                    n2/48.0 + n3/15.0,
                    17.0*n3/480.0
                };
                Delta = new[]
                {
                    2.0*N - 2.0*n2/3.0 - 2.0*n3,
                    7.0*n2/3.0 - 1.6*n3,
                    56.0*n3/15.0
                };
            }
        }

        private readonly MathConstants _m;

        private static double ATanH(double x)
        {
            return (Math.Log(1 + x) - Math.Log(1 - x)) / 2;
        }
    }
}