using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

public interface IArcGeometryHandler : IService<KmlPlacemark, Placemark, KmlGeometry, SharpKml.Dom.Geometry>
{
    SharpKml.Dom.Geometry HandleGeometry(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry);

    Task<SharpKml.Dom.Geometry> HandleGeometryAsync(KmlPlacemark kmlPlacemark, Placemark placemark, KmlGeometry kmlGeometry);
}