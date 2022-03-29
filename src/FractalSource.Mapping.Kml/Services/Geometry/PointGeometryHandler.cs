using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Geometry;

internal class PointGeometryHandler : Service<KmlPlacemark, SharpKml.Dom.Geometry>, IPointGeometryHandler
{
    public PointGeometryHandler(ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {

    }

    public SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark)
    {
        return HandleGeometryAsync(kmlPlacemark)
            .Result;
    }

    public async Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark)
    {
        await Task.CompletedTask;

        return kmlPlacemark.Coordinates.ToPoint();
    }
}