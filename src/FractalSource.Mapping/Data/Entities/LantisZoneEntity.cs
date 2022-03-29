using System.ComponentModel.DataAnnotations.Schema;

namespace FractalSource.Mapping.Data.Entities;

public class LantisZoneEntity : NamedMappingEntity
{
    public double ZoneStart { get; set; }

    public double ZoneEnd { get; set; }

    public long ZoneTypeID { get; set; }

    public string ZoneColor { get; set; }

    [NotMapped]
    public LantisZoneType ZoneType => (LantisZoneType)ZoneTypeID;
}