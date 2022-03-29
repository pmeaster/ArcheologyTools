using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Geometry;

public interface IPointGeometryHandler : IService<KmlPlacemark, SharpKml.Dom.Geometry>
{
    SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark);

    Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark);
}