using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisMeasurementSystemNetworkLinkProvider : IService<NetworkLink>
{
    NetworkLink GetNetworkLink(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useAntipode = false);
}