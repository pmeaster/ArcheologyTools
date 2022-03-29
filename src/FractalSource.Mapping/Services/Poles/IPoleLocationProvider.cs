using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.Poles;

public interface IPoleLocationProvider : IEntityProvider<PoleLocationEntity>
{
    PoleLocationEntity GetLocation(int locationId);

    Task<PoleLocationEntity> GetLocationAsync(int locationId);
}