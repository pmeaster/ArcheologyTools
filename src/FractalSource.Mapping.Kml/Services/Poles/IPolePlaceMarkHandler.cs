using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Poles;

internal interface IPolePlaceMarkHandler : IService<LocationEntity>
{
    KmlFeatureContainer HandlePlaceMark(LocationEntity location, bool isNorth = true);

    Task<KmlFeatureContainer> HandlePlaceMarkAsync(LocationEntity location, bool isNorth = true);
}