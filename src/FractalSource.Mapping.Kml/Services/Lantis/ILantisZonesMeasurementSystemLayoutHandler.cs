using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisZonesMeasurementSystemLayoutHandler : IService<LocationEntity, Feature>
{
    public const string FolderDescription = "This is a zone layout using the {1} system.{0}{0}{2}{0}{0}1 {3} = {4} m (meters).";

    Feature HandleLayout(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useNetworkLinks = false, bool useAntipode = false);

    Task<Feature> HandleLayoutAsync(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useNetworkLinks = false, bool useAntipode = false);
}