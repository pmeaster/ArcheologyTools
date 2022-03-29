using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemsLayoutHandler : IService<LocationEntity, KmlFeatureContainer>
{
    public const string LayoutName = "Solar System Layouts";
    public const string LayoutDescription = "Overlays of the solar system based on different orbital configurations and measurement systems.";

    KmlFeatureContainer HandleLayout(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false);

    Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false);
}