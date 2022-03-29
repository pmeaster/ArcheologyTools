using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemLayoutMeasurementSystemHandler : IService<LocationEntity, Feature>
{
    public const string FolderDescription = "This is a solar system layout using the {1} system.{0}{0}{2}{0}{0}1 {3} = {4} m (meters).";

    Feature HandleLayout(LocationEntity locationEntity, SolarSystemConfigurationEntity solarSystemConfiguration, 
        MeasurementSystemEntity measurementSystem);

    Task<Feature> HandleLayoutAsync(LocationEntity locationEntity, SolarSystemConfigurationEntity solarSystemConfiguration,
        MeasurementSystemEntity measurementSystem);
}