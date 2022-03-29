using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class MeasurementSystemExpandedEntity : NamedMappingEntity
    {
        public string Abbreviation { get; set; }

        [NotMapped]
        public string StadiaAbbreviation => $"k{Abbreviation}";

        public double CalibrationValue { get; set; }

        public double BaseRatio { get; set; }

        public long SystemTypeID { get; set; }

        public bool IsBaseSystem { get; set; }

        public long ParentSystemID { get; set; }

        public long AxisPositionTypeID { get; set; }

        public long AxisRadiusTypeID { get; set; }

        [NotMapped]
        public AxisRadiusType AxisRadiusType => (AxisRadiusType)AxisRadiusTypeID;
    }
}