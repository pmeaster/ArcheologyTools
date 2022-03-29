using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisTaskHandler : IService<IEnumerable<LocationEntity>, KmlDocument>
{
    KmlDocument HandleLantisLocations(IEnumerable<LocationEntity> locations);

    Task<KmlDocument> HandleLantisLocationsAsync(IEnumerable<LocationEntity> locations);
}