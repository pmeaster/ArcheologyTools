using System;
using System.Collections.Generic;
using System.Linq;
using FractalSource.Mapping.Geodesy;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping
{
    //TODO: Break out every geometry into its own handler
    public class GeoCoordinatesFactory : ServiceFactory, IGeoCoordinatesFactory
    {
        private readonly GeodeticCalculator _geodeticCalculator;
        private const int MaxNumberOfPoints = 2000;

        public GeoCoordinatesFactory(IGeodeticCalculatorFactory geodeticCalculatorFactory, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
            _geodeticCalculator = geodeticCalculatorFactory.CreateGeodeticCalculator();
        }

        public GeoCoordinates CalculateEndingGeoCoordinates(GeoCoordinates startCoordinates, double azimuthInRadians, double distanceInMeters)
        {
            return
                _geodeticCalculator
                    .CalculateEndingGeoCoordinates(startCoordinates, azimuthInRadians, distanceInMeters);
        }

        public IEnumerable<GeoCoordinates> CalculateGeodeticPath(GeoCoordinates startCoordinates, GeoCoordinates endCoordinates, int numberOfPoints = 10)
        {
            return
                _geodeticCalculator
                    .CalculateGeodeticPath(startCoordinates, endCoordinates, numberOfPoints);
        }

        public GeoCoordinatesCurve CalculateGeodeticCurve(GeoCoordinates startCoordinates, GeoCoordinates endCoordinates)
        {
            var curve = _geodeticCalculator
                .CalculateGeodeticCurve(startCoordinates, endCoordinates);

            return new GeoCoordinatesCurve
            {
                Azimuth = curve.Azimuth,
                EllipsoidalDistance = curve.EllipsoidalDistance
            };
        }

        public IEnumerable<GeoCoordinates> CreateCircle(GeoCoordinates focalPoint, double radiusInMeters,
            int numberOfPoints = 120, double degreesToRotate = 0)
        {
            return
                CreateEllipse(focalPoint,
                    radiusInMeters,
                    Constants.ZeroEccentricity,
                    numberOfPoints,
                    degreesToRotate);
        }

        public IEnumerable<GeoCoordinates> CreateArc(GeoCoordinates focalPoint, double radiusInMeters, double startAngle,
            double endAngle, double eccentricity = Constants.ZeroEccentricity, int numberOfPoints = 120,
            double degreesToRotate = 0)
        {
            var ellipseCoordinatesList = new List<GeoCoordinates>();

            var eccentricityRatio = 1 - (eccentricity * 2);
            var radiansToRotate = Angle.DegreesToRadians(degreesToRotate);

            numberOfPoints
                = (int)Math.Round(Math.Max(radiusInMeters / 1000 * numberOfPoints, numberOfPoints), 0);

            numberOfPoints = Math.Min(numberOfPoints, MaxNumberOfPoints);

            var thetaIncrement = (float)Constants.RadiansPerEllipse / numberOfPoints;

            var startTheta = Angle.DegreesToRadians(startAngle);
            var endTheta = Angle.DegreesToRadians(endAngle);

            var totalTheta = endTheta - startTheta;
            var totalPoints = (totalTheta / thetaIncrement) + 1;

            var currentTheta = startTheta;

            for (var i = 0; i <= totalPoints; i++)
            {
                var radius = radiusInMeters;

                if (!eccentricity.IsApproximatelyEqualTo(Constants.ZeroEccentricity))
                {
                    var x = (radiusInMeters * eccentricityRatio * Math.Cos(currentTheta));
                    var y = (radiusInMeters * Math.Sin(currentTheta));

                    radius
                        = Math.Sqrt(
                            Math.Pow(x, 2)
                            + Math.Pow(y, 2)
                        );
                }

                ellipseCoordinatesList.Add(
                    CalculateEndingGeoCoordinates(focalPoint, currentTheta + radiansToRotate, radius));

                currentTheta += thetaIncrement;
            }

            return ellipseCoordinatesList;
        }

        public IEnumerable<GeoCoordinates> CreateEllipse(GeoCoordinates focalPoint, double radiusInMeters,
            double eccentricityRatio = Constants.ZeroEccentricity, int numberOfPoints = 120,
            double degreesToRotate = 0, double inclinationInDegrees = 0)
        {
            var ellipseCoordinatesList = new List<GeoCoordinates>();

            var altitude = focalPoint.Altitude;

            var semiMajorAxis = radiusInMeters;
            var semiMinorAxis = semiMajorAxis * (eccentricityRatio != 0 ? eccentricityRatio : 1);

            var eccentricity = (1 - eccentricityRatio) / (1 + eccentricityRatio);

            var radiansToRotate = Angle.DegreesToRadians(degreesToRotate);
            var inclinationInRadians = Angle.DegreesToRadians(inclinationInDegrees);

            numberOfPoints
                = (int)Math.Round(Math.Max(radiusInMeters / 1000 * numberOfPoints, numberOfPoints), 0);

            numberOfPoints = Math.Min(numberOfPoints, MaxNumberOfPoints);

            var thetaIncrement = (float)Constants.RadiansPerEllipse / numberOfPoints;
            var currentTheta = 0d;
            var currentAltitude = altitude;

            for (var i = 0; i <= numberOfPoints; i++)
            {
                var radius = semiMajorAxis;

                if (!eccentricity.IsApproximatelyEqualTo(0)
                    && !eccentricity.IsApproximatelyEqualTo(1))
                {

                    radius = semiMajorAxis * (1 - Math.Pow(eccentricity, 2)) /
                                 (1 + eccentricity * Math.Cos(currentTheta));
                }

                if (!inclinationInRadians.IsApproximatelyEqualTo(0))
                {
                    var reverseInclination = Angle.DegreesToRadians(90 - inclinationInDegrees);
                    var angleRatio = Math.Sin(inclinationInRadians) / Math.Sin(reverseInclination);

                    var startOffset = angleRatio * semiMinorAxis;

                    var currentRadiusRatio = radius / semiMajorAxis;
                    var diameter = semiMajorAxis + semiMinorAxis;

                    var theta
                        = currentTheta > Math.PI
                            ? Math.PI - (currentTheta - Math.PI)
                            : currentTheta;

                    var percentOfDiameter = theta * (1 / Math.PI);

                    var currentRun = (diameter * currentRadiusRatio) * percentOfDiameter;

                    var currentOffset = (angleRatio * currentRun) - startOffset;

                    currentAltitude = altitude + currentOffset;
                }

                //TODO: Add elevation data to every coordinate collection

                var newCoordinate = CalculateEndingGeoCoordinates(focalPoint,
                    currentTheta + radiansToRotate,
                    radius);

                newCoordinate.Altitude = currentAltitude;

                ellipseCoordinatesList.Add(newCoordinate);

                currentTheta += thetaIncrement;
            }

            return ellipseCoordinatesList;
        }

        public IEnumerable<IEnumerable<GeoCoordinates>> CreateSpheroid(GeoCoordinates focalPoint, double radiusInMeters,
            double eccentricity = Constants.ZeroEccentricity, int numberOfPoints = 120,
            int numberOfEllipses = 60, double degreesToRotate = 0)
        {
            var eccentricityRatio = 1 - (eccentricity * 2);
            var radiansToRotate = Angle.DegreesToRadians(degreesToRotate);

            numberOfPoints
                = (int)Math.Round(Math.Max(radiusInMeters / 1000 * numberOfPoints, numberOfPoints), 0);

            numberOfPoints = Math.Min(numberOfPoints, MaxNumberOfPoints);

            var thetaIncrement = (float)Constants.RadiansPerEllipse / numberOfPoints;
            var ellipseIncrement = radiusInMeters / numberOfEllipses;

            var lists = new List<List<GeoCoordinates>>();

            for (var currentRadius = ellipseIncrement; currentRadius <= radiusInMeters; currentRadius += ellipseIncrement)
            {
                var ellipseCoordinatesList = new List<GeoCoordinates>();

                var currentTheta = 0d;

                for (var i = 0; i <= numberOfPoints; i++)
                {
                    var ellipseRadius = currentRadius;
                    var azimuth = currentTheta + radiansToRotate;

                    var outerEllipseRadius = radiusInMeters;

                    if (!eccentricity.IsApproximatelyEqualTo(Constants.ZeroEccentricity))
                    {
                        var x = (currentRadius * eccentricityRatio * Math.Cos(currentTheta));
                        var y = (currentRadius * Math.Sin(currentTheta));

                        ellipseRadius
                            = Math.Sqrt(
                                Math.Pow(x, 2)
                                + Math.Pow(y, 2)
                            );

                        var x2 = (radiusInMeters * eccentricityRatio * Math.Cos(currentTheta));
                        var y2 = (radiusInMeters * Math.Sin(currentTheta));

                        outerEllipseRadius
                            = Math.Sqrt(
                                Math.Pow(x2, 2)
                                + Math.Pow(y2, 2)
                            );
                    }

                    var coordinates = CalculateEndingGeoCoordinates(focalPoint,
                        azimuth,
                        ellipseRadius);

                    var altitude = Math.Sqrt(
                            Math.Pow(outerEllipseRadius, 2) -
                            Math.Pow(ellipseRadius, 2)
                        );

                    coordinates.Altitude = altitude - 150 /* Altitude seems to be off by about 150 meters in Google Earth */;

                    if (coordinates.Altitude >= 0)
                    {
                        ellipseCoordinatesList.Add(coordinates);
                    }

                    currentTheta += thetaIncrement;
                }

                if (ellipseCoordinatesList.Count > 0)
                {
                    var minimumAltitude = ellipseCoordinatesList.Min(coordinates => coordinates.Altitude);
                    ellipseCoordinatesList.ForEach(coordinates => coordinates.Altitude = minimumAltitude);
                }

                lists.Add(ellipseCoordinatesList);
            }

            return lists;
        }
    }
}