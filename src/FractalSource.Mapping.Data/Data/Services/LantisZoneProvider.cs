using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class LantisZoneProvider : Service<LantisZoneEntity>, ILantisZoneProvider
{
    private readonly IRepository<LantisZoneEntity> _repository;

    public LantisZoneProvider(ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory
            .CreateRepository<LantisZoneEntity>();
    }

    public IEnumerable<LantisZoneEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    public async Task<IEnumerable<LantisZoneEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<LantisZoneEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var poleLocations = await _repository.GetAllAsync(cancellationToken);

        return
            poleLocations as LantisZoneEntity[]
            ?? poleLocations.ToArray();
    }
}