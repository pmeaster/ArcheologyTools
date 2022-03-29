using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class SolarSystemObjectRadiusProvider : Service<SolarSystemObjectRadiusEntity>, ISolarSystemObjectRadiusProvider
{
    private readonly IArcheologyContextFactory _archeologyContextFactory;

    public SolarSystemObjectRadiusProvider(IArcheologyContextFactory archeologyContextFactory,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _archeologyContextFactory = archeologyContextFactory;
    }

    public IEnumerable<SolarSystemObjectRadiusEntity> GetRecords(long configurationID, AxisType axisType)
    {
        return GetRecordsAsync(configurationID, axisType)
            .Result;
    }

    public async Task<IEnumerable<SolarSystemObjectRadiusEntity>> GetRecordsAsync(long configurationID, AxisType axisType,
        CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(configurationID, axisType, cancellationToken);
    }

    protected virtual async Task<IEnumerable<SolarSystemObjectRadiusEntity>> OnGetRecordsAsync(long configurationID,
        AxisType axisType, CancellationToken cancellationToken = default)
    {
        await using var context = _archeologyContextFactory.CreateContext();

        var solarObjects
            = await context.GetSolarSystemObjectRadii(configurationID, (int)axisType).ToListAsync(cancellationToken);

        var objectsArray 
            = solarObjects.OrderBy(obj => obj.AvgPRatioRadius);

        return objectsArray.ToArray();
    }
}