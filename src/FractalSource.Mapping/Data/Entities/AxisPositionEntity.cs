#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class AxisPositionEntity : NamedMappingEntity
    {
        public double SemiMajorAxis { get; set; }

        public double SemiMinorAxis { get; set; }

        public long PositionTypeID { get; set; }
    }
}