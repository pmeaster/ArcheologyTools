namespace FractalSource.Mapping.Keyhole;

public abstract class KmlShape : KmlItem
{
    public virtual int PointsCount { get; set; } = 120;

    public bool? Fill { get; set; }

    public bool? Outline { get; set; }
}