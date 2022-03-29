using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal interface ISolarSystemOrbitBoundaryHandler : IService<LocationEntity, Placemark>
{
    Placemark HandleOrbitBoundary(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius,
        AxisRadiusType axisRadiusType);

    Task<Placemark> HandleOrbitBoundaryAsync(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius,
        AxisRadiusType axisRadiusType);
}