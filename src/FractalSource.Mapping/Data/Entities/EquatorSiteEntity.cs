#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class EquatorSiteEntity : MappingEntity
    {
        public string SiteName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double North { get; set; }

        public double East { get; set; }
    }
}