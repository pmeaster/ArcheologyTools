namespace FractalSource.Data;

public abstract class IndexedRecord : Record, IIndexedRecord
{
    public long ID { get; set; }
}