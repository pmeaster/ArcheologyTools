#nullable disable
namespace FractalSource.Mapping.Data.Entities
{
    public class SolarSystemObjectEntity : NamedMappingEntity
    {
        public long ObjectTypeID { get; set; }

        public long ParentObjectID { get; set; }

        public string ObjectColor { get; set; }

        public double OrbitAltitude { get; set; }

        public double OrbitRotation { get; set; }

        public double OrbitInclination { get; set; }
    }
}