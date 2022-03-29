using System.Xml.Linq;

namespace FractalSource.Mapping.Keyhole;

public class KmlDocument : KmlItem
{
    public XDocument XDocument { get; set; }
}