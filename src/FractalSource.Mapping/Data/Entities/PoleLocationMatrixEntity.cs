#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class PoleLocationMatrixEntity : MappingEntity
    {
        public int SiteID { get; set; }

        public long PoleLocationID { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Bearing { get; set; }

        public double Azimuth { get; set; }

        public double ReverseAzimuth { get; set; }
    }
}