#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class SitePoleLocationAlignmentTypeEntity : MappingEntity
    {
        public long? SiteID { get; set; }

        public long? AlignmentTypeID { get; set; }

        public long? PoleLocationID { get; set; }
    }
}