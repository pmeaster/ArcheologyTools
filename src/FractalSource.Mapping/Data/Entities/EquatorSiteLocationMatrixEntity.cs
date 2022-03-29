#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class EquatorSiteLocationMatrixEntity : MappingEntity
    {
        public int SiteID { get; set; }

        public long PoleLocationID { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Distance { get; set; }
    }
}