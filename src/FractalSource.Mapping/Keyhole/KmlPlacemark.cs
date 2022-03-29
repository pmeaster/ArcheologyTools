namespace FractalSource.Mapping.Keyhole;

public class KmlPlacemark : KmlItem
{
    public GeoCoordinates Coordinates { get; set; }

    public KmlPlacemarkType PlacemarkType { get; set; }

    public KmlPlacemarkStyle Style { get; set; } = new();
}