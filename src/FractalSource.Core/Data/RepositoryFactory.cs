using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public class RepositoryFactory : ServiceFactory, IRepositoryFactory
    {
        public RepositoryFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected IRepository<TEntity> OnCreateRepository<TEntity>()
            where TEntity : class, IRecord
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IRepository<TEntity>>(ServiceProvider);
        }

        public IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IRecord
        {
            return
                OnCreateRepository<TEntity>();
        }

        public IRepository<TEntity> CreateRepository<TEntity>(ServiceKey<TEntity> serviceKey)
            where TEntity : class, IRecord
        {
            return
                OnCreateRepository<TEntity>();
        }

        public IRepository<TEntity> CreateRepository<TEntity>(Func<ServiceKey<TEntity>> serviceKeyDelegate)
            where TEntity : class, IRecord
        {
            return
                OnCreateRepository<TEntity>();
        }
    }
}