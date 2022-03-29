using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Data;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemConfigurationProvider : IEntityProvider<SolarSystemConfigurationEntity>
{
    SolarSystemConfigurationEntity GetSolarSystemConfiguration(int solarSystemConfigurationId);

    Task<SolarSystemConfigurationEntity> GetSolarSystemConfigurationAsync(int solarSystemConfigurationId);
}