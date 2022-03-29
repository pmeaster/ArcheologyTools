using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Mapping.Services.Sites;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class LocationProvider : Service<LocationEntity>, ILocationProvider
{
    private readonly IPoleLocationProvider _poleLocationProvider;
    private readonly ISiteLocationProvider _siteLocationProvider;
    private readonly ILantisLocationProvider _lantisLocationProvider;

    public LocationProvider(IPoleLocationProvider poleLocationProvider, ISiteLocationProvider siteLocationProvider, ILantisLocationProvider lantisLocationProvider,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _poleLocationProvider = poleLocationProvider;
        _siteLocationProvider = siteLocationProvider;
        _lantisLocationProvider = lantisLocationProvider;
    }

    public IEnumerable<LocationEntity> GetRecords(LocationType locationType)
    {
        return GetRecordsAsync(locationType)
            .Result;
    }

    public async Task<IEnumerable<LocationEntity>> GetRecordsAsync(LocationType locationType, CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(locationType, cancellationToken);
    }

    protected virtual async Task<IEnumerable<LocationEntity>> OnGetRecordsAsync(LocationType locationType, CancellationToken cancellationToken = default)
    {
        switch (locationType)
        {
            case LocationType.Lantis:
                return await _lantisLocationProvider.GetRecordsAsync(cancellationToken);

            case LocationType.Pole:
                return await _poleLocationProvider.GetRecordsAsync(cancellationToken);

            case LocationType.Site:
                return await _siteLocationProvider.GetRecordsAsync(cancellationToken);

            default:
                throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null);
        }
    }

    public LocationEntity GetLocation(int locationId, LocationType locationType)
    {
        return GetLocationAsync(locationId, locationType)
            .Result;
    }

    public async Task<LocationEntity> GetLocationAsync(int locationId, LocationType locationType)
    {
        switch (locationType)
        {
            case LocationType.Lantis:
                return await _lantisLocationProvider.GetLocationAsync(locationId);

            case LocationType.Pole:
                return await _poleLocationProvider.GetLocationAsync(locationId);

            case LocationType.Site:
                return await _siteLocationProvider.GetLocationAsync(locationId);

            default:
                throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null);
        }
    }
}