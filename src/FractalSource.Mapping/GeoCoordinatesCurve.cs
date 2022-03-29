using FractalSource.Services;

namespace FractalSource.Mapping;

public class GeoCoordinatesCurve : ServiceItem
{
    public double EllipsoidalDistance { get; set; }

    public Angle Azimuth { get; set; }

}