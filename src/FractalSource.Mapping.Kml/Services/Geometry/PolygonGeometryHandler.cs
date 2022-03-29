using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Geometry;

internal class PolygonGeometryHandler : Service<KmlPlacemark, KmlGeometry, SharpKml.Dom.Geometry>, IPolygonGeometryHandler
{
    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;

    public PolygonGeometryHandler(IGeoCoordinatesFactory geoCoordinatesFactory, ILoggerFactory loggerFactory)
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

        var coordinatesList = new List<List<GeoCoordinates>>();

        foreach (var systemRadius in kmlGeometry.Radii)
        {
            var radiusInMeters = systemRadius * kmlGeometry.MeasurementSystemRatio;

            var coordinates
                = _geoCoordinatesFactory.CreateEllipse(
                    kmlPlacemark.Coordinates, 
                    radiusInMeters,
                    kmlGeometry.Ellipse.Eccentricity,
                    kmlGeometry.Polygon.PointsCount,
                    kmlGeometry.Ellipse.DegreesToRotate,
                    kmlGeometry.Ellipse.Inclination);

            coordinatesList.Add(
                coordinates.ToList());
        }

        return coordinatesList.To3DPolygon(
            kmlGeometry.Altitude.AltitudeMode,
            kmlGeometry.Altitude.Extrude,
            kmlGeometry.UseMultiGeometry);
    }
}