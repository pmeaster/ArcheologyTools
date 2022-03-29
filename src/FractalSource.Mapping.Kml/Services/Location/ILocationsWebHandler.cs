using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationsWebHandler : IService<LocationEntity, Feature>
{
    Feature HandleLocations(LocationType locationType, bool useNetworkLinks = false);

    Task<Feature> HandleLocationsAsync(LocationType locationType, bool useNetworkLinks = false);
}