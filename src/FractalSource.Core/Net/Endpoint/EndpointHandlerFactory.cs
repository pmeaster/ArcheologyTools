using System;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointHandlerFactory<TConstraint> : ServiceFactory, IEndpointHandlerFactory<TConstraint>
        where TConstraint : class, IRecord
    {
        protected EndpointHandlerFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected virtual IEndpointHandler<TRecord> OnCreateEndpointHandler<TRecord>()
            where TRecord : class, TConstraint
        {
            return ActivatorUtilities.GetServiceOrCreateInstance<IEndpointHandler<TRecord>>(ServiceProvider);
        }

        public IEndpointHandler<TRecord> CreateEndpointHandler<TRecord>(ServiceKey<TRecord> serviceKey)
            where TRecord : class, TConstraint
        {
            return OnCreateEndpointHandler<TRecord>();
        }
    }
}