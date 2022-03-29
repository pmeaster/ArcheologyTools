using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationCompassLayoutHandler : IService<LocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandleLocationCompassLayout(LocationEntity location);

    Task<KmlFeatureContainer> HandleLocationCompassLayoutAsync(LocationEntity location);
}