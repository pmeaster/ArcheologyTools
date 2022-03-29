using FractalSource.Mapping;
using FractalSource.Mapping.Geodesy;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
// ReSharper disable UnusedMember.Global

public class SqlGeodeticFunctions
{
    [SqlFunction(Name = nameof(CalculateGeodeticCurveAzimuth))]
    public static double CalculateGeodeticCurveAzimuth(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
    {
        var start = new GeoCoordinates(startLatitude, startLongitude);
        var end = new GeoCoordinates(endLatitude, endLongitude);

        var curve = new GeodeticCalculator(Ellipsoid.Wgs84).CalculateGeodeticCurve(start, end);

        return curve.Azimuth.Degrees;
    }

    [SqlFunction(Name = nameof(CalculateGeodeticCurveDistance))]
    public static double CalculateGeodeticCurveDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
    {
        var start = new GeoCoordinates(startLatitude, startLongitude);
        var end = new GeoCoordinates(endLatitude, endLongitude);

        var curve = new GeodeticCalculator(Ellipsoid.Wgs84).CalculateGeodeticCurve(start, end);

        return curve.EllipsoidalDistance;
    }

    [SqlFunction(
        Name = nameof(GeneratePoleLocationMatrix),
        FillRowMethodName = nameof(GeneratePoleLocationMatrixFillRow),
        TableDefinition = "ID bigint, Latitude float, Longitude float")]
    public static IEnumerable GeneratePoleLocationMatrix(double startLatitide, double endLatitude,
        double startLongitude, double endLongitude, double latitudeIncrement, double longitudeIncrement)
    {
        var coordinates = new List<SqlGeoCoordinates>();
        var order = 1;

        for (var currentLatitude = startLatitide;
            currentLatitude <= endLatitude;
            currentLatitude += latitudeIncrement)
        {
            for (var currentLongitude = startLongitude;
                currentLongitude <= endLongitude;
                currentLongitude += longitudeIncrement)
            {
                coordinates.Add(new SqlGeoCoordinates(currentLatitude, currentLongitude, order));

                order++;
            }
        }

        return coordinates;
    }

    public static void GeneratePoleLocationMatrixFillRow(object coordinateObj,
        out SqlInt64 id, out SqlDouble latiutude, out SqlDouble longitude)
    {
        var coordinate = (SqlGeoCoordinates)coordinateObj;

        id = new SqlInt64(coordinate.Order);
        latiutude = new SqlDouble(coordinate.Latitude ?? 0);
        longitude = new SqlDouble(coordinate.Longitude ?? 0);
    }

    /*
    [SqlFunction(Name = nameof(CalculateIntersectDistance))]
    public static double? CalculateIntersectDistance(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude,
        double firstAngleInDegrees, double secondAngleInDegrees)
    {
        var firstAngleInRadians = Angle.DegreesToRadians(firstAngleInDegrees);
        var secondAngleInRadians = Angle.DegreesToRadians(secondAngleInDegrees);

        var firstSin  = Math.Sin(firstAngleInRadians);
        var firstCos  = Math.Cos(firstAngleInRadians);

        var secondSin  = Math.Sin(secondAngleInRadians);
        var secondCos  = Math.Cos(secondAngleInRadians);

        var determinant = (secondSin * firstCos) - (firstSin * secondCos);

        if (determinant.IsApproximatelyEqualTo(0d))
        {
            return null;
        }

        var latitudeDelta2  = secondLatitude - firstLatitude;
        var longitudeDelta2 = secondLongitude - firstLongitude;

        var latitudeDelta
            = new GeodeticCalculator(Ellipsoid.Wgs84)
                .CalculateGeodeticCurve(
                    new GeoCoordinates(firstLatitude, 0),
                    new GeoCoordinates(secondLatitude, 0)
                ).EllipsoidalDistance;

        var longitudeDelta
            = new GeodeticCalculator(Ellipsoid.Wgs84)
                .CalculateGeodeticCurve(
                    new GeoCoordinates(0, firstLongitude),
                    new GeoCoordinates(0, secondLongitude)
                ).EllipsoidalDistance;

        var firstDistance = ((secondSin * latitudeDelta) - (secondCos * longitudeDelta)) / determinant;
        var secondDistance = ((secondSin * latitudeDelta) - (secondCos * longitudeDelta)) / determinant;

        return secondDistance < 0d || firstDistance < 0d ? null : firstDistance ;
    }

    [SqlFunction(Name = nameof(CalculateGeodeticCurve))]
    public static SqlGeodeticCurve CalculateGeodeticCurve(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
    {
        return CalculateGeodeticCurveFromCoordinates(
            new SqlGeoCoordinates(startLatitude, startLongitude),
            new SqlGeoCoordinates(endLatitude, endLongitude)
            );
    }

    [SqlFunction(Name = nameof(CalculateGeodeticCurveFromCoordinates))]
    public static SqlGeodeticCurve CalculateGeodeticCurveFromCoordinates(SqlGeoCoordinates startCoordinates, SqlGeoCoordinates endCoordinates)
    {
        var start = new GeoCoordinates(startCoordinates.Latitude ?? 0, startCoordinates.Longitude ?? 0);
        var end = new GeoCoordinates(endCoordinates.Latitude ?? 0, endCoordinates.Longitude ?? 0);

        var curve = new GeodeticCalculator(Ellipsoid.Wgs84).CalculateGeodeticCurve(start, end);

        return new SqlGeodeticCurve(curve.Azimuth.Degrees, curve.EllipsoidalDistance);
    }

    [SqlFunction(Name = nameof(CalculateEndingCoordinates))]
    public static SqlGeoCoordinates CalculateEndingCoordinates(SqlGeoCoordinates startCoordinates, double angleInDegrees, double distance)
    {
        var angleInRadians = Angle.DegreesToRadians(angleInDegrees);

        var start = new GeoCoordinates(startCoordinates.Latitude ?? 0, startCoordinates.Longitude ?? 0);

        var coordinates
            = new GeodeticCalculator(Ellipsoid.Wgs84).CalculateEndingGeoCoordinates(start, angleInRadians, distance);

        return new SqlGeoCoordinates(coordinates.Latitude, coordinates.Longitude);
    }
    */
}
