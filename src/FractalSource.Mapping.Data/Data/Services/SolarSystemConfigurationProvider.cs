using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class SolarSystemConfigurationProvider 
    : Service<SolarSystemConfigurationEntity>, ISolarSystemConfigurationProvider
{
    private readonly IRepository<SolarSystemConfigurationEntity> _repository;

    public SolarSystemConfigurationProvider(IRepositoryFactory repositoryFactory, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory.CreateRepository<SolarSystemConfigurationEntity>();
    }

    public IEnumerable<SolarSystemConfigurationEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    public async Task<IEnumerable<SolarSystemConfigurationEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<SolarSystemConfigurationEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var configurations = await _repository.GetAllAsync(cancellationToken);

        var configurationsArray
            = configurations as SolarSystemConfigurationEntity[] ?? configurations.ToArray();

        return configurationsArray;
    }

    public SolarSystemConfigurationEntity GetSolarSystemConfiguration(int solarSystemConfigurationId)
    {
        return GetSolarSystemConfigurationAsync(solarSystemConfigurationId)
            .Result;
    }

    public async Task<SolarSystemConfigurationEntity> GetSolarSystemConfigurationAsync(int solarSystemConfigurationId)
    {
        var configs
            = await _repository.GetAsync(config => config.ID == solarSystemConfigurationId);

        try
        {
            var config = configs.First();

            return config;
        }
        catch (InvalidOperationException e)
        {
            Logger.LogError(e.Message);

            throw new ArgumentException(
                "The specified value is not a valid solar system configuration identifier.", nameof(solarSystemConfigurationId));
        }
    }
}