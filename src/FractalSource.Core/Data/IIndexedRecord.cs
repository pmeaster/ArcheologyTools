namespace FractalSource.Data;

public interface IIndexedRecord : IRecord
{
    public long ID { get; set; }
}