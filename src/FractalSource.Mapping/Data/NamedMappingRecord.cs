namespace FractalSource.Mapping.Data;

public class NamedMappingRecord : MappingRecord, INamedMappingRecord
{
    public string Name { get; set; }

    public string Description { get; set; }
}