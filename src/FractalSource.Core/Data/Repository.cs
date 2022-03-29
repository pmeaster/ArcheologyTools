using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable InconsistentNaming

namespace FractalSource.Data
{
    public abstract class Repository<TEntity, TContext> : Service<TEntity>, IRepository<TEntity>
        where TEntity : class, IRecord
        where TContext : DbContext
    {
        protected Repository(TContext dbContext, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Context = dbContext;
        }

        protected TContext Context { get; }

        private async Task OnBulkCommandInternal(IEnumerable<TEntity> entities, BulkCommandType commandType,
            CancellationToken cancellationToken = default)
        {
            Logger.LogMethodStart($"{nameof(OnBulkCommand)}:{commandType}");
            await OnBulkCommand(entities, commandType, cancellationToken);
            Logger.LogMethodEnd($"{nameof(OnBulkCommand)}:{commandType}");
        }

        protected abstract Task OnBulkCommand(IEnumerable<TEntity> entities, BulkCommandType commandType,
            CancellationToken cancellationToken = default);

        protected virtual async Task OnBulkInsert(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkCommandInternal(entities, BulkCommandType.Insert, cancellationToken);
        }

        protected virtual async Task OnBulkUpdate(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await
                OnBulkCommandInternal(entities, BulkCommandType.Update, cancellationToken);
        }

        protected virtual async Task OnBulkDelete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await
                OnBulkCommandInternal(entities, BulkCommandType.Delete, cancellationToken);
        }

        protected virtual async Task OnBulkMerge(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await
                OnBulkCommandInternal(entities, BulkCommandType.Merge, cancellationToken);
        }

        protected virtual async Task OnBulkSynchronize(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await
                OnBulkCommandInternal(entities, BulkCommandType.Synchronize, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await
                Context.Set<TEntity>()
                    .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereClause, CancellationToken cancellationToken = default)
        {
            return
                await Context.Set<TEntity>()
                    .Where(whereClause)
                    .ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(long recordID, DateTime? dateTime, CancellationToken cancellationToken = default)
        {
            return await
                Context.Set<TEntity>()
                    .FindAsync(dateTime.HasValue
                        ? new object[]
                        {
                            recordID,
                            dateTime
                        }
                        : new object[]
                        {
                            recordID
                        }, cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>()
                .AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkInsert(entities, cancellationToken);
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkUpdate(entities, cancellationToken);
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkDelete(entities, cancellationToken);
        }

        public async Task BulkMergeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkMerge(entities, cancellationToken);
        }

        public async Task BulkSynchronizeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await OnBulkSynchronize(entities, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Context.Entry(entity)
                .State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(long symbolID, DateTime? dateTime, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<TEntity>()
                .FindAsync(dateTime.HasValue
                    ? new object[]
                    {
                        symbolID,
                        dateTime
                    }
                    : new object[]
                    {
                        symbolID
                    }, cancellationToken);

            if (entity != null)
                Context.Set<TEntity>()
                    .Remove(entity);

            await Context.SaveChangesAsync(cancellationToken);
        }

        public enum BulkCommandType
        {
            Delete = 0,
            Insert = 1,
            Merge = 2,
            Synchronize = 3,
            Update = 4
        }
    }
}