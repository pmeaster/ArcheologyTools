using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles;

internal interface IPoleParallelsHandler : IService<LocationEntity>
{
    KmlFeatureContainer HandlePoleLocationParallels(LocationEntity location);

    Task<KmlFeatureContainer> HandlePoleLocationParallelsAsync(LocationEntity location);
}