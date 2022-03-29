using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.Sites;

public interface ISiteLocationProvider : IEntityProvider<SiteLocationEntity>
{
    SiteLocationEntity GetLocation(int locationId);

    Task<SiteLocationEntity> GetLocationAsync(int locationId);
}