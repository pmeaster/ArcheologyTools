using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class MeasurementSystemEntity : NamedMappingEntity
    {
        public string Abbreviation { get; set; }

        [NotMapped]
        public string StadiaAbbreviation => $"k{Abbreviation}";

        public double CalibrationValue { get; set; }

        public double BaseRatio { get; set; }

        public long SystemTypeID { get; set; }

        public bool IsBaseSystem { get; set; }

        public long CategoryID { get; set; }

        [NotMapped]
        public MeasurementSystemCategory Category => (MeasurementSystemCategory)CategoryID;
    }
}