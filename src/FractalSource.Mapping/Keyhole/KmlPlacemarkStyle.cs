namespace FractalSource.Mapping.Keyhole;

public class KmlPlacemarkStyle : KmlItem
{
    public KmlIcon Icon { get; set; } = new();

    public double? LabelSize { get; set; }
}