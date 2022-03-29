using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationHandler : IService<LocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandleLocation(LocationEntity location);

    Task<KmlFeatureContainer> HandleLocationAsync(LocationEntity location);
}