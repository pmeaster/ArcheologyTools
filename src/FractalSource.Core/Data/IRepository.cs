using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;

// ReSharper disable InconsistentNaming

namespace FractalSource.Data
{
    public interface IRepository<TEntity> : IService<TEntity>
        where TEntity : class, IRecord
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereClause, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(long recordID, DateTime? dateTime, CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task BulkUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task BulkDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task BulkMergeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task BulkSynchronizeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(long symbolID, DateTime? dateTime, CancellationToken cancellationToken = default);
    }
}