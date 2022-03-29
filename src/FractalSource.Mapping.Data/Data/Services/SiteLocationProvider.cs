using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Sites;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class SiteLocationProvider : Service<SiteLocationEntity>, ISiteLocationProvider
{
    private readonly IRepository<SiteLocationEntity> _repository;

    public SiteLocationProvider(ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory
            .CreateRepository<SiteLocationEntity>();
    }

    public IEnumerable<SiteLocationEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    public async Task<IEnumerable<SiteLocationEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<SiteLocationEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var locations = (await _repository.GetAllAsync(cancellationToken)).ToList();

        locations.Sort((location1, location2)
            => string.Compare(
                location1.Name,
                location2.Name,
                StringComparison.InvariantCultureIgnoreCase)
            );

        return
            locations;
    }

    public SiteLocationEntity GetLocation(int locationId)
    {
        return GetLocationAsync(locationId)
            .Result;
    }

    public async Task<SiteLocationEntity> GetLocationAsync(int locationId)
    {
        var locations
            = await _repository.GetAsync(location => location.ID == locationId);

        try
        {
            var location = locations.First();

            return location;
        }
        catch (InvalidOperationException e)
        {
            Logger.LogError(e.Message);

            throw new ArgumentException(
                "The specified value is not a valid location identifier.", nameof(locationId));
        }
    }
}
