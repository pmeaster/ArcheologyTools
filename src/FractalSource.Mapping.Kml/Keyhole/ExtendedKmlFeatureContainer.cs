using System.ComponentModel.DataAnnotations.Schema;
using SharpKml.Dom;

namespace FractalSource.Mapping.Keyhole;

public class ExtendedKmlFeatureContainer : KmlFeatureContainer
{

    [NotMapped]
    public Feature Feature { get; set; }
}