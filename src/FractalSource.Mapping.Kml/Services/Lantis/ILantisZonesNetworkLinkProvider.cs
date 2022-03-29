using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisZonesNetworkLinkProvider : IService<NetworkLink>
{
    NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false);
}