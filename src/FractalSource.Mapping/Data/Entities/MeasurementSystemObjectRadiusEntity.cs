#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class MeasurementSystemObjectRadiusEntity : MappingEntity
    {
        public long MeasurementSystemExpandedID { get; set; }

        public long SolarSystemObjectRadiusID { get; set; }

        public double Value { get; set; }
    }
}