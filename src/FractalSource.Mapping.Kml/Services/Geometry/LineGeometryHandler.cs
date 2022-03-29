using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Geometry;

internal class LineGeometryHandler : Service<KmlPlacemark, KmlGeometry, SharpKml.Dom.Geometry>, ILineGeometryHandler
{
    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;

    public LineGeometryHandler(IGeoCoordinatesFactory geoCoordinatesFactory, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _geoCoordinatesFactory = geoCoordinatesFactory;
    }

    public SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry)
    {
        return HandleGeometryAsync(kmlPlacemark, kmlGeometry)
            .Result;
    }

    public async Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry)
    {
        await Task.CompletedTask;

        var radiusInMeters = kmlGeometry.Radii.FirstOrDefault() * kmlGeometry.MeasurementSystemRatio;

        return _geoCoordinatesFactory
            .CreateEllipse(
                kmlPlacemark.Coordinates, 
                radiusInMeters, 
                kmlGeometry.Ellipse.Eccentricity, 
                kmlGeometry.Ellipse.PointsCount, 
                kmlGeometry.Ellipse.DegreesToRotate,
                kmlGeometry.Ellipse.Inclination)
            .ToLine(
                kmlGeometry.Altitude.AltitudeMode,
                kmlGeometry.Altitude.Extrude,
                kmlGeometry.Altitude.DrawOrder);
    }
}