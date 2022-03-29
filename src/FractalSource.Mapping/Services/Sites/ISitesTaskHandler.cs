using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Sites;

public interface ISitesTaskHandler : IService<IEnumerable<LocationEntity>, KmlDocument>
{
    KmlDocument HandleSiteLocations(IEnumerable<LocationEntity> locations);

    Task<KmlDocument> HandleSiteLocationsAsync(IEnumerable<LocationEntity> locations);
}