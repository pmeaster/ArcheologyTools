using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemLayoutHandler : IService<LocationEntity, Feature> 
{
    Feature HandleLayout(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, 
        bool useNetworkLinks = false, bool useAntipode = false);

    Task<Feature> HandleLayoutAsync(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, 
        bool useNetworkLinks = false, bool useAntipode = false);
}