using FractalSource.Data;
using FractalSource.Mapping.Data.Context;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Repository;

internal class SqlRepository<TEntity> : Repository<TEntity, ArcheologyContext>, ISqlRepository<TEntity>
    where TEntity : class, IEntity
{
    public SqlRepository(ArcheologyContext archeologyContext, ILoggerFactory loggerFactory)
        : base(archeologyContext, loggerFactory)
    {
    }

    protected override async Task OnBulkCommand(IEnumerable<TEntity> entities, BulkCommandType commandType, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}