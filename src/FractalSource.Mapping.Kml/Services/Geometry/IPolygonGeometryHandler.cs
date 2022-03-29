using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Geometry;

public interface IPolygonGeometryHandler : IService<KmlPlacemark, KmlGeometry, SharpKml.Dom.Geometry>
{
    SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry);

    Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry);
}