using System;

// ReSharper disable IdentifierTypo

namespace FractalSource.Mapping.Geodesy
{
    /// <summary>
    ///     Implementation of Thaddeus Vincenty's algorithms to solve the direct and
    ///     inverse geodetic problems.  For more information, see Vincent's original
    ///     publication on the NOAA website:
    ///     See http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
    /// </summary>
    public readonly struct GeodeticCalculator : IEquatable<GeodeticCalculator>
    {
        public static readonly GeodeticCalculator Default;

        static GeodeticCalculator()
        {
            Default = new GeodeticCalculator();
        }

        /// <summary>
        ///     The reference Ellipsoid to use for the calculations
        /// </summary>
        public Ellipsoid ReferenceGlobe { get; }

        /// <summary>
        ///     Instantiate a calculator for the specified reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe">The reference Ellipsoid</param>
        public GeodeticCalculator(Ellipsoid referenceGlobe)
        {
            ReferenceGlobe = referenceGlobe;
        }

        /// <summary>
        ///     Calculate the destination and final bearing after traveling a specified
        ///     distance, and a specified starting bearing, for an initial location.
        ///     This is the solution to the direct geodetic problem.
        /// </summary>
        /// <param name="start">starting location</param>
        /// <param name="angleInRadians">starting bearing (radians)</param>
        /// <param name="distance">distance to travel (meters)</param>
        /// <returns>The coordinates of the final location of the traveling</returns>
        public GeoCoordinates CalculateEndingGeoCoordinates(GeoCoordinates start, double angleInRadians, double distance)
        {
            var majorAxis = ReferenceGlobe.SemiMajorAxis;
            var minorAxis = ReferenceGlobe.SemiMinorAxis;
            var aSquared = majorAxis * majorAxis;
            var bSquared = minorAxis * minorAxis;
            var flattening = ReferenceGlobe.Flattening;
            var phi1 = start.Latitude.Radians;
            var cosAlpha1 = Math.Cos(angleInRadians);
            var sinAlpha1 = Math.Sin(angleInRadians);
            var tanU1 = (1.0 - flattening) * Math.Tan(phi1);
            var cosU1 = 1.0 / Math.Sqrt(1.0 + tanU1 * tanU1);
            var sinU1 = tanU1 * cosU1;

            if (Math.Sign(distance) < 0)
                throw new ArgumentOutOfRangeException(nameof(distance));

            // eq. 1
            var sigma1 = Math.Atan2(tanU1, cosAlpha1);

            // eq. 2
            var sinAlpha = cosU1 * sinAlpha1;

            var sin2Alpha = sinAlpha * sinAlpha;
            var cos2Alpha = 1 - sin2Alpha;
            var uSquared = cos2Alpha * (aSquared - bSquared) / bSquared;

            // eq. 3
            var a = 1 + (uSquared / 16384) * (4096 + uSquared * (-768 + uSquared * (320 - 175 * uSquared)));

            // eq. 4
            var b = (uSquared / 1024) * (256 + uSquared * (-128 + uSquared * (74 - 47 * uSquared)));

            // iterate until there is a negligible change in sigma
            double sinSigma;
            double sigmaM2;
            double cosSigmaM2;
            double cos2SigmaM2;

            var sOverbA = distance / (minorAxis * a);
            var sigma = sOverbA;
            var prevSigma = sOverbA;

            while (true)
            {
                // eq. 5
                sigmaM2 = 2.0 * sigma1 + sigma;
                cosSigmaM2 = Math.Cos(sigmaM2);
                cos2SigmaM2 = cosSigmaM2 * cosSigmaM2;
                sinSigma = Math.Sin(sigma);
                var cosSignma = Math.Cos(sigma);

                // eq. 6
                var deltaSigma = b * sinSigma * (cosSigmaM2 + (b / 4.0) * (cosSignma * (-1 + 2 * cos2SigmaM2)
                                                                           -
                                                                           (b / 6.0) * cosSigmaM2 * (-3 + 4 * sinSigma * sinSigma) *
                                                                           (-3 + 4 * cos2SigmaM2)));

                // eq. 7
                sigma = sOverbA + deltaSigma;

                // break after converging to tolerance
                if (sigma.IsApproximatelyEqualTo(prevSigma))
                    break;
                prevSigma = sigma;
            }

            sigmaM2 = 2.0 * sigma1 + sigma;
            cosSigmaM2 = Math.Cos(sigmaM2);
            cos2SigmaM2 = cosSigmaM2 * cosSigmaM2;

            var cosSigma = Math.Cos(sigma);
            sinSigma = Math.Sin(sigma);

            // eq. 8
            var phi2 = Math.Atan2(sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1,
                (1.0 - flattening) * Math.Sqrt(sin2Alpha +
                                               Math.Pow(sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1, 2.0)));

            // eq. 9
            // This fixes the pole crossing defect spotted by Matt Feemster.  When a path
            // passes a pole and essentially crosses a line of latitude twice - once in
            // each direction - the longitude calculation got messed up.  Using Atan2
            // instead of Atan fixes the defect.  The change is in the next 3 lines.
            //double tanLambda = sinSigma * sinAlpha1 / (cosU1 * cosSigma - sinU1*sinSigma*cosAlpha1);
            //double lambda = Math.Atan(tanLambda);
            var lambda = Math.Atan2(sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);

            // eq. 10
            var c = (flattening / 16) * cos2Alpha * (4 + flattening * (4 - 3 * cos2Alpha));

            // eq. 11
            var l = lambda - (1 - c) * flattening * sinAlpha *
                (sigma + c * sinSigma * (cosSigmaM2 + c * cosSigma * (-1 + 2 * cos2SigmaM2)));

            var latitudeDegrees = Angle.RadiansToDegrees(phi2);
            var longitudeDegrees = Angle.RadiansToDegrees(start.Longitude.Radians + l);

            return new GeoCoordinates(latitudeDegrees, longitudeDegrees);
        }

        /// <summary>
        ///     Calculate the geodetic curve between two points on a specified reference ellipsoid.
        ///     This is the solution to the inverse geodetic problem.
        /// </summary>
        /// <param name="start">starting coordinates</param>
        /// <param name="end">ending coordinates </param>
        /// <returns>The geodetic curve information to get from start to end</returns>
        public GeodeticCurve CalculateGeodeticCurve(GeoCoordinates start, GeoCoordinates end)
        {
            //
            // All equation numbers refer back to Vincenty's publication:
            // See http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
            //

            // get constants
            var majorAxis = ReferenceGlobe.SemiMajorAxis;
            var minorAxis = ReferenceGlobe.SemiMinorAxis;
            var flattening = ReferenceGlobe.Flattening;

            // get parameters as radians
            var phi1 = start.Latitude.Radians;
            var lambda1 = start.Longitude.Radians;
            var phi2 = end.Latitude.Radians;
            var lambda2 = end.Longitude.Radians;

            // calculations
            var a2 = majorAxis * majorAxis;
            var b2 = minorAxis * minorAxis;
            var squaredRatio = (a2 - b2) / b2;

            var omega = lambda2 - lambda1;

            var tanphi1 = Math.Tan(phi1);
            var tanU1 = (1.0 - flattening) * tanphi1;
            var u1 = Math.Atan(tanU1);
            var sinU1 = Math.Sin(u1);
            var cosU1 = Math.Cos(u1);

            var tanphi2 = Math.Tan(phi2);
            var tanU2 = (1.0 - flattening) * tanphi2;
            var u2 = Math.Atan(tanU2);
            var sinU2 = Math.Sin(u2);
            var cosU2 = Math.Cos(u2);

            var sinU1SinU2 = sinU1 * sinU2;
            var cosU1SinU2 = cosU1 * sinU2;
            var sinU1CosU2 = sinU1 * cosU2;
            var cosU1CosU2 = cosU1 * cosU2;

            // eq. 13
            var lambda = omega;

            // intermediates we'll need to compute 's'
            var a = 0.0;
            var sigma = 0.0;
            var deltasigma = 0.0;
            var converged = false;

            for (var i = 0; i < 20; i++)
            {
                var lambda0 = lambda;

                var sinlambda = Math.Sin(lambda);
                var coslambda = Math.Cos(lambda);

                // eq. 14
                var sin2Sigma = (cosU2 * sinlambda * cosU2 * sinlambda) +
                                Math.Pow(cosU1SinU2 - sinU1CosU2 * coslambda, 2.0);
                var sinsigma = Math.Sqrt(sin2Sigma);

                // eq. 15
                var cossigma = sinU1SinU2 + (cosU1CosU2 * coslambda);

                // eq. 16
                sigma = Math.Atan2(sinsigma, cossigma);

                // eq. 17    Careful!  sin2sigma might be almost 0!
                var sinalpha = sin2Sigma.IsZero() ? 0.0 : cosU1CosU2 * sinlambda / sinsigma;
                var alpha = Math.Asin(sinalpha);
                var cosalpha = Math.Cos(alpha);
                var cos2Alpha = cosalpha * cosalpha;

                // eq. 18    Careful!  cos2alpha might be almost 0!
                var cos2Sigmam = cos2Alpha.IsZero() ? 0.0 : cossigma - 2 * sinU1SinU2 / cos2Alpha;
                var u3 = cos2Alpha * squaredRatio;

                var cos2Sigmam2 = cos2Sigmam * cos2Sigmam;

                // eq. 3
                a = 1.0 + u3 / 16384 * (4096 + u3 * (-768 + u3 * (320 - 175 * u3)));

                // eq. 4
                var b = u3 / 1024 * (256 + u3 * (-128 + u3 * (74 - 47 * u3)));

                // eq. 6
                deltasigma = b * sinsigma * (cos2Sigmam + b / 4 *
                    (cossigma * (-1 + 2 * cos2Sigmam2) - b / 6 * cos2Sigmam *
                        (-3 + 4 * sin2Sigma) * (-3 + 4 * cos2Sigmam2)));

                // eq. 10
                var c = flattening / 16 * cos2Alpha * (4 + flattening * (4 - 3 * cos2Alpha));

                // eq. 11 (modified)
                lambda = omega + (1 - c) * flattening * sinalpha *
                    (sigma + c * sinsigma * (cos2Sigmam + c * cossigma * (-1 + 2 * cos2Sigmam2)));

                // see how much improvement we got
                var change = Math.Abs((lambda - lambda0) / lambda);

                if ((i <= 1) || (!(change < Constants.DefaultPrecision))) continue;
                converged = true;
                break;
            }

            // eq. 19
            var s = minorAxis * a * (sigma - deltasigma);

            double angle;
            // didn't converge?  must be N/S
            if (!converged)
            {
                if (phi1 > phi2)
                {
                    angle = 180.0;
                }
                else if (phi1 < phi2)
                {
                    angle = 0.0;
                }
                else
                {
                    angle = double.NaN;
                }
            }
            // else, it converged, so do the math
            else
            {
                // eq. 20
                var radians = Math.Atan2(
                    cosU2 * Math.Sin(lambda),
                    (cosU1SinU2 - sinU1CosU2 * Math.Cos(lambda)));

                if (radians.IsNegative())
                {
                    radians += Constants.TwoTimesPi;
                }

                angle = radians / Constants.PiOver180;

            }

            if (angle >= 360.0) angle -= 360.0;

            return new GeodeticCurve(this, s, new Angle(angle));
        }

        /// <summary>
        ///     Calculate the three dimensional geodetic measurement between two positions
        ///     measured in reference to a specified ellipsoid.
        ///     This calculation is performed by first computing a new ellipsoid by expanding or contracting
        ///     the reference ellipsoid such that the new ellipsoid passes through the average elevation
        ///     of the two positions.  A geodetic curve across the new ellipsoid is calculated.  The
        ///     point-to-point distance is calculated as the hypotenuse of a right triangle where the length
        ///     of one side is the ellipsoidal distance and the other is the difference in elevation.
        /// </summary>
        /// <param name="start">starting position</param>
        /// <param name="end">ending position</param>
        /// <returns>The geodetic measurement information to get from start to end</returns>
        public GeodeticMeasurement CalculateGeodeticMeasurement(GeoCoordinates start, GeoCoordinates end)
        {
            // calculate elevation differences
            var elev1 = start.Altitude;
            var elev2 = end.Altitude;
            var elev12 = (elev1 + elev2) / 2.0;

            // calculate latitude differences
            var phi1 = start.Latitude.Radians;
            var phi2 = end.Latitude.Radians;
            var phi12 = (phi1 + phi2) / 2.0;

            // calculate a new ellipsoid to accommodate average elevation
            var refA = ReferenceGlobe.SemiMajorAxis;
            var f = ReferenceGlobe.Flattening;
            var a = refA + elev12 * (1.0 + f * Math.Sin(phi12));
            var ellipsoid = new Ellipsoid(a, f);
            var geoCalc = new GeodeticCalculator(ellipsoid);

            // calculate the curve at the average elevation
            var averageCurve =
                geoCalc.CalculateGeodeticCurve(start, end);

            // return the measurement
            return new GeodeticMeasurement(averageCurve, elev2 - elev1);
        }

        /// <summary>
        ///     Compute a geodetic path from start to end with a given number of points
        /// </summary>
        /// <param name="start">starting coordinates</param>
        /// <param name="end">ending coordinates</param>
        /// <param name="numberOfPoints">Number of points on the path (including start and end)</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number of points is less than 2</exception>
        /// <returns>An array of points describing the path from start to end</returns>
        public GeoCoordinates[] CalculateGeodeticPath(GeoCoordinates start, GeoCoordinates end, int numberOfPoints = 10)
        {
            if (numberOfPoints < 2)
                throw new ArgumentOutOfRangeException(nameof(numberOfPoints));
            if (start == end || numberOfPoints == 2)
                return new[] { start, end };

            var curve = CalculateGeodeticCurve(start, end);
            var stepWidth = curve.EllipsoidalDistance / (numberOfPoints - 1);
            var result = new GeoCoordinates[numberOfPoints];
            result[0] = start;
            result[numberOfPoints - 1] = end;
            for (var i = 1; i < numberOfPoints - 1; i++)
            {
                result[i] = CalculateEndingGeoCoordinates(result[i - 1],
                    curve.Azimuth.Radians,
                    stepWidth);
            }
            return result;
        }

        public bool Equals(GeodeticCalculator other)
        {
            return ReferenceGlobe.Equals(other.ReferenceGlobe);
        }
    }
}