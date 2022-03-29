using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

internal class SphereGeometryHandler : Service<KmlPlacemark, Placemark, KmlGeometry, SharpKml.Dom.Geometry>, ISphereGeometryHandler
{
    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;

    public SphereGeometryHandler(IGeoCoordinatesFactory geoCoordinatesFactory, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _geoCoordinatesFactory = geoCoordinatesFactory;
    }

    public SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry)
    {
        return HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry)
            .Result;
    }

    public async Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry)
    {
        await Task.CompletedTask;

        var sphere = kmlGeometry.Sphere;

        var style = new Style
        {
            Polygon = new PolygonStyle
            {
                Fill = sphere.Fill,
                Outline = sphere.Outline
            }
        };

        placemark.AddStyle(style);

        var radiusInMeters =
            kmlGeometry.Radii.FirstOrDefault() * kmlGeometry.MeasurementSystemRatio;

        return _geoCoordinatesFactory
            .CreateSpheroid(
                kmlPlacemark.Coordinates,
                radiusInMeters,
                sphere.Eccentricity,
                sphere.PointsCount,
                sphere.EllipseCount,
                sphere.DegreesToRotate)
            .ToSphere(
                kmlGeometry.Altitude.AltitudeMode == KmlAltitudeMode.Absolute
                    ? KmlAltitudeMode.Absolute
                    : KmlAltitudeMode.RelativeToGround,
                sphere.Extrude);
    }
}