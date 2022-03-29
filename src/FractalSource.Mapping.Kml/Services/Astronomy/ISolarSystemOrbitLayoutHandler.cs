using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal interface ISolarSystemOrbitLayoutHandler : IService<LocationEntity, Folder>
{
    Folder HandleLayout(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius);

    Task<Folder> HandleLayoutAsync(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius);
}