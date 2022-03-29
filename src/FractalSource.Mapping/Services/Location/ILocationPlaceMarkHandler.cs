using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationPlaceMarkHandler : IService<LocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandlePlaceMark(LocationEntity location, bool isAntipode = false);

    Task<KmlFeatureContainer> HandlePlaceMarkAsync(LocationEntity location, bool isAntipode = false);
}