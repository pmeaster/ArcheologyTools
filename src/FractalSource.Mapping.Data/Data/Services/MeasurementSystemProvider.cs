using FractalSource.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.MeasurementSystem;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class MeasurementSystemProvider : Service<MeasurementSystemEntity>, IMeasurementSystemProvider
{
    private readonly IRepository<MeasurementSystemEntity> _repository;

    public MeasurementSystemProvider(ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory)
        : base(loggerFactory)
    {
        _repository = repositoryFactory
            .CreateRepository<MeasurementSystemEntity>();
    }

    public IEnumerable<MeasurementSystemEntity> GetRecords()
    {
        return GetRecordsAsync()
            .Result;
    }

    //TODO: Add CancellationToken param to all Async methods and pass the token.
    public async Task<IEnumerable<MeasurementSystemEntity>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await OnGetRecordsAsync(cancellationToken);
    }

    protected virtual async Task<IEnumerable<MeasurementSystemEntity>> OnGetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var measurementSystems = (await _repository.GetAllAsync(cancellationToken)).ToList();

        measurementSystems.Sort((system1, system2)
            => string.Compare(
                system1.Name,
                system2.Name,
                StringComparison.InvariantCultureIgnoreCase)
        );

        return measurementSystems;
    }

    public MeasurementSystemEntity GetMeasurementSystem(int measurementSystemId)
    {
        return GetMeasurementSystemAsync(measurementSystemId)
            .Result;
    }

    public async Task<MeasurementSystemEntity> GetMeasurementSystemAsync(int measurementSystemId)
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

