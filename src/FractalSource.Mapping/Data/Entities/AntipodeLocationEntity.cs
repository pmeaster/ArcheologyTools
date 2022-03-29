namespace FractalSource.Mapping.Data.Entities;

internal class AntipodeLocationEntity : LocationEntity
{
    public AntipodeLocationEntity(LocationEntity location)
    {
        var antipode = location.Coordinates.GetAntipode();

        Name = $"{location.Name} (Antipode)";
        Description = $"{location.Description} (Antipode)";
        Latitude = antipode.Latitude;
        Longitude = antipode.Longitude; 
        Coordinates = antipode;
        Altitude = antipode.Altitude;
        LineColor = location.LineColor;
    }
}