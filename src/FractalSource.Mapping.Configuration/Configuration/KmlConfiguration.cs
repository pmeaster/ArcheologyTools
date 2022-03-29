using FractalSource.Mapping.Keyhole;

namespace FractalSource.Mapping.Configuration
{
    public class KmlConfiguration : IKmlConfiguration
    {
        //private readonly IConfiguration _configuration;

        //public KmlConfiguration(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    Name = IKmlConfiguration.SectionName;

        //    //var sectionKey = $"{Name}:{nameof(Layouts)}";
        //    //var section = configuration.GetSection(sectionKey);

        //    //Layouts = section
        //    //    .Get<KmlConfigurationItem[]>()
        //    //    ?.ToList();
        //}

        public string Name { get; set; }

        //public IEnumerable<KmlConfigurationItem> Layouts { get; }
    }
}