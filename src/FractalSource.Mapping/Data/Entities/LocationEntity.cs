using System.ComponentModel.DataAnnotations.Schema;

namespace FractalSource.Mapping.Data.Entities;

//TODO: Create Models for Entities
public abstract class LocationEntity : NamedMappingEntity
{
    [NotMapped]
    public GeoCoordinates Coordinates
    {
        get => new(Latitude, Longitude, Altitude ?? 0);
        set
        {
            {
                Latitude = value?.Latitude ?? 0;
                Longitude = value?.Longitude ?? 0;
                Altitude = value?.Altitude ?? 0;
            }
        }
    }

    [NotMapped]
    public LocationType LocationType { get; set; }

    public string LineColor { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double? Altitude { get; set; }
}