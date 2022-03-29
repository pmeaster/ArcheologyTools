
using System.ComponentModel.DataAnnotations.Schema;

namespace FractalSource.Mapping.Data.Entities
{
    public class SolarSystemObjectRadiusEntity : NamedMappingEntity
    {
        public double MinRadius { get; set; }

        public double MaxRadius { get; set; }

        public double AvgRadius { get; set; }

        public double MinPRatioRadius { get; set; }

        public double MaxPRatioRadius { get; set; }

        public double AvgPRatioRadius { get; set; }

        public string ObjectBaseColor { get; set; }

        public long AxisTypeID { get; set; }

        public long ConfigurationID { get; set; }

        public double OrbitAltitude { get; set; }

        public double OrbitRotation { get; set; }

        public double OrbitInclination { get; set; }

        [NotMapped]
        public SolarSystemConfigurationEntity Configuration { get; set; }

        [NotMapped]
        public MeasurementSystemEntity MeasurementSystem { get; set; }

        public long ObjectTypeID { get; set; }

        [NotMapped]
        public AxisType AxisType => (AxisType)AxisTypeID;

        [NotMapped]
        public SolarSystemObjectType ObjectType => (SolarSystemObjectType)ObjectTypeID;
    }
}
