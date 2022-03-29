using FractalSource.Configuration;

namespace FractalSource.Mapping.Keyhole
{
    public interface IKmlConfiguration : IHasConfigurationName
    {
        public static string SectionName => nameof(IKmlConfiguration);

        //IEnumerable<KmlConfigurationItem> Layouts { get; }
    }
}