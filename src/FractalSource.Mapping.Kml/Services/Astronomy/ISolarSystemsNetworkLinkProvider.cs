using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemsNetworkLinkProvider : IService<NetworkLink>
{
    NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false);
}