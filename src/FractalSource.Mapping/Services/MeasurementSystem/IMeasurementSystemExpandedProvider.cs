using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.MeasurementSystem;

public interface IMeasurementSystemExpandedProvider : IEntityProvider<MeasurementSystemExpandedEntity>
{
    MeasurementSystemExpandedEntity GetMeasurementSystem(int measurementSystemId);

    Task<MeasurementSystemExpandedEntity> GetMeasurementSystemAsync(int measurementSystemId);
}