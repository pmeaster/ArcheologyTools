namespace FractalSource.Mapping.Keyhole;

public class KmlArc : KmlEllipse
{
    public double? InnerRadius { get; set; }

    public double? OuterRadius { get; set; }

    public double? StartAngle { get; set; }

    public double? EndAngle { get; set; }
}