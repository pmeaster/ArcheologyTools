using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles;

public interface IPoleLayoutHandler : IService<LocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandleLayout(LocationEntity location);

    Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location);
}