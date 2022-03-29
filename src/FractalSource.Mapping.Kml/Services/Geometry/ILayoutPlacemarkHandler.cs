using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Geometry;

public interface ILayoutPlacemarkHandler : IService<KmlPlacemark, KmlGeometry, Placemark>
{
    Placemark HandlePlaceMark(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry);

    Task<Placemark> HandlePlaceMarkAsync(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry);
}