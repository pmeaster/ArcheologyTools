using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationLayoutHandler : IService<LocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandleLayout(LocationEntity location);

    Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location);
}