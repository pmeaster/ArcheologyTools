#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class MeasurementSystemObjectShapeEntity : MappingEntity
    {
        public long MeasurementSystemObjectRadiusID { get; set; }

        public double Radius { get; set; }

        public double Diameter { get; set; }

        public double Circumference { get; set; }

        public double Area { get; set; }

        public double Volume { get; set; }
    }
}