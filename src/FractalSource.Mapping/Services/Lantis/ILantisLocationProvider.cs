using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisLocationProvider : IEntityProvider<LantisLocationEntity>
{
    LantisLocationEntity GetLocation(int locationId);

    Task<LantisLocationEntity> GetLocationAsync(int locationId);
}