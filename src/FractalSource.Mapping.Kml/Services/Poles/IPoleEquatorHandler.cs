using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles;

internal interface IPoleEquatorHandler : IService<LocationEntity>
{
    KmlFeatureContainer HandlePoleLocationEquator(LocationEntity location);

    Task<KmlFeatureContainer> HandlePoleLocationEquatorAsync(LocationEntity location);
}