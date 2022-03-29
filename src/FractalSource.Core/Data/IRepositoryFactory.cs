using System;
using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRepositoryFactory : IServiceFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>() 
            where TEntity : class, IRecord;

        IRepository<TEntity> CreateRepository<TEntity>(ServiceKey<TEntity> serviceKey)
            where TEntity : class, IRecord;

        IRepository<TEntity> CreateRepository<TEntity>(Func<ServiceKey<TEntity>> serviceKeyDelegate)
            where TEntity : class, IRecord;
    }
}