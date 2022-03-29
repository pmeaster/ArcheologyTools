using System.Collections.Generic;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Geodesy
{
    public interface IGeoCoordinatesFactory : IServiceFactory
    {
        GeoCoordinates CalculateEndingGeoCoordinates(GeoCoordinates startCoordinates, double azimuthInRadians, 
            double distanceInMeters);

        IEnumerable<GeoCoordinates> CalculateGeodeticPath(GeoCoordinates startCoordinates, GeoCoordinates endCoordinates, 
            int numberOfPoints = 10);

        GeoCoordinatesCurve CalculateGeodeticCurve(GeoCoordinates startCoordinates, GeoCoordinates endCoordinates);

        IEnumerable<GeoCoordinates> CreateCircle(GeoCoordinates focalPoint, double radiusInMeters, int numberOfPoints = 120, 
            double degreesToRotate = 0);

        IEnumerable<GeoCoordinates> CreateEllipse(GeoCoordinates focalPoint, double radiusInMeters, 
            double eccentricityRatio = Constants.ZeroEccentricity, int numberOfPoints = 120, 
            double degreesToRotate = 0, double inclinationInDegrees = 0);

        IEnumerable<GeoCoordinates> CreateArc(GeoCoordinates focalPoint, double radiusInMeters, 
            double startAngle, double endAngle, double eccentricity = Constants.ZeroEccentricity, 
            int numberOfPoints = 120, double degreesToRotate = 0);

        IEnumerable<IEnumerable<GeoCoordinates>> CreateSpheroid(GeoCoordinates focalPoint, double radiusInMeters,
           double eccentricity = Constants.ZeroEccentricity, int numberOfPoints = 120,
           int numberOfEllipses = 60, double degreesToRotate = 0);
    }
}