using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationWebHandler : IService<LocationEntity, Feature>
{
    Feature HandleLocation(LocationEntity location, bool useNetworkLinks = false, bool includeAntipode = true);

    Task<Feature> HandleLocationAsync(LocationEntity location, bool useNetworkLinks = false, bool includeAntipode = true);
}