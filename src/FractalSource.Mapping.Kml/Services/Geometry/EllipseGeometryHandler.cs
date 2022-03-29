using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

internal class EllipseGeometryHandler : Service<KmlPlacemark, Placemark, KmlGeometry, SharpKml.Dom.Geometry>, IEllipseGeometryHandler
{
    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;

    public EllipseGeometryHandler(IGeoCoordinatesFactory geoCoordinatesFactory, ILoggerFactory loggerFactory)
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

        var ellipse = kmlGeometry.Ellipse;

        var style = new Style
        {
            Polygon = new PolygonStyle
            {
                Fill = ellipse.Fill,
                Outline = ellipse.Outline
            }
        };

        placemark.AddStyle(style);

        var coordinatesList = new List<List<GeoCoordinates>>();

        foreach (var systemRadius in kmlGeometry.Radii)
        {
            var radiusInMeters = systemRadius * kmlGeometry.MeasurementSystemRatio;

            coordinatesList.Add(_geoCoordinatesFactory
                .CreateEllipse(
                    kmlPlacemark.Coordinates,
                    radiusInMeters,
                    ellipse.Eccentricity,
                    ellipse.PointsCount,
                    ellipse.DegreesToRotate,
                    ellipse.Inclination)
                .ToList()
            );
        }

        return coordinatesList.To3DPolygon(
            kmlGeometry.Altitude.AltitudeMode,
            kmlGeometry.Altitude.Extrude,
            kmlGeometry.UseMultiGeometry);
    }
}