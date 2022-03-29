using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Keyhole;

public interface INetworkLinkProvider : IService<LocationEntity, NetworkLink>
{
    NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false);
}