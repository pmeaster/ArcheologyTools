namespace FractalSource.Mapping.Keyhole;

public class KmlAltitude : KmlItem
{
    public KmlAltitudeMode AltitudeMode { get; set; } = KmlAltitudeMode.ClampToGround;

    public bool Extrude { get; set; }

    public int? DrawOrder { get; set; }
}