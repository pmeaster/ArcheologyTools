namespace FractalSource.Data;

public interface INamedRecord : IRecord
{
    public string Name { get; set; }

    public string Description { get; set; }
}