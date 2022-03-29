using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles;

internal interface IPoleMeridiansHandler : IService<LocationEntity>
{
    KmlFeatureContainer HandlePoleLocationMeridians(LocationEntity location);

    Task<KmlFeatureContainer> HandlePoleLocationMeridiansAsync(LocationEntity location);
}