using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles
{
    public interface IPolesTaskHandler : IService<IEnumerable<LocationEntity>, KmlDocument>
    {
        KmlDocument HandlePoleLocations(IEnumerable<LocationEntity> locations);

        Task<KmlDocument> HandlePoleLocationsAsync(IEnumerable<LocationEntity> locations);
    }
}