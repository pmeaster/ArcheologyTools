using System.Collections.Generic;

namespace FractalSource.Mapping.Keyhole;

public class KmlGeometry : KmlItem
{
    public KmlAltitude Altitude { get; set; } = new();

    public KmlArc Arc { get; set; } = new();

    public KmlEllipse Ellipse { get; set; } = new();

    public KmlLineStyle LineStyle { get; set; } = new();

    public double MeasurementSystemRatio { get; set; } = 1;

    public KmlPolygon Polygon { get; set; } = new();

    public KmlPolygonStyle PolygonStyle { get; set; } = new();

    public IEnumerable<double> Radii { get; set; } = new List<double>();

    public KmlSphere Sphere { get; set; } = new();

    public bool UseMultiGeometry { get; set; }
}