using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisZonesLayoutHandler : IService<LocationEntity, KmlFeatureContainer>
{
    public const string LayoutName = "Lantis Zone Layouts";
    public const string LayoutDescription = "An overlay of the concentric zones of Atlantis and other \"Lantis\" locations.";

    KmlFeatureContainer HandleLayout(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false);

    Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false);
}