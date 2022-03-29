using FractalSource.Data;

namespace FractalSource.Mapping.Data.Repository;

public interface ISqlRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
}