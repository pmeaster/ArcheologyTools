using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.MeasurementSystem;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class MeasurementSystemExpandedProvider : Service<MeasurementSystemExpandedEntity>, IMeasurementSystemExpandedProvider
{
    private readonly IRepository<MeasurementSystemExpandedEntity> _repository;

    public MeasurementSystemExpandedProvider(ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory.CreateRepository<MeasurementSystemExpandedEntity>();
    }

    public IEnumerable<MeasurementSystemExpandedEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    public async Task<IEnumerable<MeasurementSystemExpandedEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<MeasurementSystemExpandedEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var systems = await _repository.GetAllAsync(cancellationToken);

        return
            systems as MeasurementSystemExpandedEntity[]
            ?? systems.ToArray();
    }

    public MeasurementSystemExpandedEntity GetMeasurementSystem(int measurementSystemId)
    {
        return GetMeasurementSystemAsync(measurementSystemId)
            .Result;
    }

    public async Task<MeasurementSystemExpandedEntity> GetMeasurementSystemAsync(int measurementSystemId)
    {
        var systems
            = await _repository.GetAsync(system => system.ID == measurementSystemId);

        try
        {
            var system = systems.First();

            return system;
        }
        catch (InvalidOperationException e)
        {
            Logger.LogError(e.Message);

            throw new ArgumentException(
                "The specified value is not a valid measurement system identifier.", nameof(measurementSystemId));
        }
    }
}