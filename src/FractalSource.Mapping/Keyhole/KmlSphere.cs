namespace FractalSource.Mapping.Keyhole;

public class KmlSphere : KmlEllipse
{
    public int EllipseCount { get; set; } = 60;

    public bool Extrude { get; set; }
}