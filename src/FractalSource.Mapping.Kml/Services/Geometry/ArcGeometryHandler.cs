using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

internal class ArcGeometryHandler : Service<KmlPlacemark, Placemark, KmlGeometry, SharpKml.Dom.Geometry>, IArcGeometryHandler
{
    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;

    public ArcGeometryHandler(IGeoCoordinatesFactory geoCoordinatesFactory, ILoggerFactory loggerFactory)
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

        var arc = kmlGeometry.Arc;

        var style = new Style
        {
            Polygon = new PolygonStyle
            {
                Fill = arc.Fill,
                Outline = arc.Outline
            }
        };

        placemark.AddStyle(style);

        var outerRadiusInMeters =
            (arc.OuterRadius ?? 0) * kmlGeometry.MeasurementSystemRatio;

        var innerRadiusInMeters =
            (arc.InnerRadius ?? 0) * kmlGeometry.MeasurementSystemRatio;


        var outerCoordinates
            = _geoCoordinatesFactory.CreateArc(
                kmlPlacemark.Coordinates,
                outerRadiusInMeters,
                arc.StartAngle ?? 0,
                arc.EndAngle ?? 360,
                arc.Eccentricity,
                arc.PointsCount,
                arc.DegreesToRotate).ToList();

        var innerCoordinates
            = _geoCoordinatesFactory.CreateArc(
                kmlPlacemark.Coordinates,
                innerRadiusInMeters,
                arc.StartAngle ?? 0,
                arc.EndAngle ?? 360,
                arc.Eccentricity,
                arc.PointsCount,
                arc.DegreesToRotate)
            .Reverse().ToList();

        var arcHeader =
            _geoCoordinatesFactory.CalculateGeodeticPath(
                innerCoordinates.LastOrDefault(),
                outerCoordinates.FirstOrDefault(),
                20);

        var arcFooter =
            _geoCoordinatesFactory.CalculateGeodeticPath(
                innerCoordinates.FirstOrDefault(),
                outerCoordinates.LastOrDefault(),
                20);

        var coordinatesList = new List<IEnumerable<GeoCoordinates>>
            {
                arcHeader,
                outerCoordinates,
                arcFooter,
                innerCoordinates
            };

        return coordinatesList
            .ToPolygon(
                kmlGeometry.Altitude.AltitudeMode,
                kmlGeometry.Altitude.Extrude);
    }
}