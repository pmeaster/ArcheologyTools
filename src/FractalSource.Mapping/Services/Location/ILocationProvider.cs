using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationProvider : IService<LocationEntity>
{
    IEnumerable<LocationEntity> GetRecords(LocationType locationType);

    Task<IEnumerable<LocationEntity>> GetRecordsAsync(LocationType locationType, CancellationToken cancellationToken = default);

    LocationEntity GetLocation(int locationId, LocationType locationType);

    Task<LocationEntity> GetLocationAsync(int locationId, LocationType locationType);
}
