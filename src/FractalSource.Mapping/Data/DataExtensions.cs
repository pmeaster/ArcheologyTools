using FractalSource.Mapping.Data.Entities;

namespace FractalSource.Mapping.Data;

public static class DataExtensions
{
    public static LocationEntity GetAntipode(this LocationEntity location)
    {
        return new AntipodeLocationEntity(location);
    }

    public static LocationEntity ConvertToAntipode(this LocationEntity location)
    {
        var antipode = location.Coordinates.GetAntipode();

        location.Name = $"{location.Name} (Antipode)";
        location.Coordinates = antipode;

        return location;
    }
}