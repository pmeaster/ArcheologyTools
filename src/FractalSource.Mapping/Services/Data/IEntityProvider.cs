using FractalSource.Data;

namespace FractalSource.Mapping.Services.Data;

public interface IEntityProvider<TRecord> : IRecordProvider<TRecord>
    where TRecord : class, IEntity
{
}