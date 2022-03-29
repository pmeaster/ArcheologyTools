#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class EquatorSiteLocationMatrixSummaryEntity : MappingEntity
    {
        public long PoleLocationID { get; set; }

        public int SiteCount { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}