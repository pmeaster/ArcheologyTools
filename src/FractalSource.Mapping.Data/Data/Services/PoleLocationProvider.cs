using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class PoleLocationProvider : Service<PoleLocationEntity>, IPoleLocationProvider
{
    private readonly IRepository<PoleLocationEntity> _repository;

    public PoleLocationProvider(ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory
            .CreateRepository<PoleLocationEntity>();
    }

    public IEnumerable<PoleLocationEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    public async Task<IEnumerable<PoleLocationEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<PoleLocationEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var poleLocations = await _repository.GetAllAsync(cancellationToken);

        return
            poleLocations as PoleLocationEntity[]
            ?? poleLocations.ToArray();
    }

    public PoleLocationEntity GetLocation(int poleLocationId)
    {
        return GetLocationAsync(poleLocationId)
            .Result;
    }

    public async Task<PoleLocationEntity> GetLocationAsync(int poleLocationId)
    {
        var locations
            = await _repository.GetAsync(location => location.ID == poleLocationId);

        try
        {
            var location = locations.First();

            return location;
        }
        catch (InvalidOperationException e)
        {
            Logger.LogError(e.Message);

            throw new ArgumentException(
                "The specified value is not a valid location identifier.", nameof(poleLocationId));
        }
    }
}