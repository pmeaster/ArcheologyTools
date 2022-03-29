using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemObjectRadiusProvider : IService<SolarSystemObjectRadiusEntity>
{
    IEnumerable<SolarSystemObjectRadiusEntity> GetRecords(long configurationID, AxisType axisType);

    Task<IEnumerable<SolarSystemObjectRadiusEntity>> GetRecordsAsync(long configurationID, AxisType axisType, 
        CancellationToken cancellationToken = default);
}