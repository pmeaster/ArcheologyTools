using System;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

internal class LayoutGeometryHandler : Service<KmlPlacemark, Placemark, KmlGeometry, SharpKml.Dom.Geometry>, ILayoutGeometryHandler
{
    private readonly IPointGeometryHandler _pointGeometryHandler;
    private readonly ILineGeometryHandler _lineGeometryHandler;
    private readonly IPolygonGeometryHandler _polygonGeometryHandler;
    private readonly IArcGeometryHandler _arcGeometryHandler;
    private readonly IEllipseGeometryHandler _ellipseGeometryHandler;
    private readonly ISphereGeometryHandler _sphereGeometryHandler;

    public LayoutGeometryHandler(IPointGeometryHandler pointGeometryHandler, ILineGeometryHandler lineGeometryHandler, 
        IPolygonGeometryHandler polygonGeometryHandler, IArcGeometryHandler arcGeometryHandler, 
        IEllipseGeometryHandler ellipseGeometryHandler, ISphereGeometryHandler sphereGeometryHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _pointGeometryHandler = pointGeometryHandler;
        _lineGeometryHandler = lineGeometryHandler;
        _polygonGeometryHandler = polygonGeometryHandler;
        _arcGeometryHandler = arcGeometryHandler;
        _ellipseGeometryHandler = ellipseGeometryHandler;
        _sphereGeometryHandler = sphereGeometryHandler;
    }

    public SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry)
    {
        return HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry)
            .Result;
    }

    public async Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry)
    {
        var geometry = kmlPlacemark.PlacemarkType switch
        {
            KmlPlacemarkType.Point => await _pointGeometryHandler.HandleGeometryAsync(kmlPlacemark),

            KmlPlacemarkType.Line => await _lineGeometryHandler.HandleGeometryAsync(kmlPlacemark, kmlGeometry),

            KmlPlacemarkType.Polygon => await _polygonGeometryHandler.HandleGeometryAsync(kmlPlacemark, kmlGeometry),

            KmlPlacemarkType.Arc => await _arcGeometryHandler.HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry),

            KmlPlacemarkType.Ellipse => await _ellipseGeometryHandler.HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry),

            KmlPlacemarkType.Sphere => await _sphereGeometryHandler.HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry),

            _ => throw new ArgumentOutOfRangeException()
        };

        return geometry;
    }
}