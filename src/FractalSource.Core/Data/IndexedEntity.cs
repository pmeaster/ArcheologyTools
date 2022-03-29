namespace FractalSource.Data;

public abstract class IndexedEntity : Entity, IIndexedEntity
{
    public long ID { get; set; }
}