using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.MeasurementSystem;

public interface IMeasurementSystemProvider : IEntityProvider<MeasurementSystemEntity>
{
    MeasurementSystemEntity GetMeasurementSystem(int measurementSystemId);

    Task<MeasurementSystemEntity> GetMeasurementSystemAsync(int measurementSystemId);
}