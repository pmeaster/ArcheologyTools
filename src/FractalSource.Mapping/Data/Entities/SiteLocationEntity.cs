#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class SiteLocationEntity : LocationEntity
    {
        public SiteLocationEntity()
        {
            LocationType = LocationType.Site;
        }

        public long RegionID { get; set; }

        public string CityState { get; set; }

        public string Country { get; set; }

        public double? North { get; set; }

        public double? East { get; set; }
    }
}