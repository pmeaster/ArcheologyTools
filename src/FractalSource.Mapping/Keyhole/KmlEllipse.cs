namespace FractalSource.Mapping.Keyhole;

public class KmlEllipse : KmlShape
{
    public double Eccentricity { get; set; } = Constants.ZeroEccentricity;

    public double DegreesToRotate { get; set; } = 0;

    public double Inclination { get; set; } = 0;
}