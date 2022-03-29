using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemMeasurementSystemNetworkLinkProvider : IService<NetworkLink>
{
    NetworkLink GetNetworkLink(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, 
        MeasurementSystemEntity measurementSystem, bool useAntipode = false);
}