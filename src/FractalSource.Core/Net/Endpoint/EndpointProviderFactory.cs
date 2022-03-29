using System;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public class EndpointProviderFactory : ServiceFactory, IEndpointProviderFactory
    {
        public EndpointProviderFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected IEndpointProvider<TRecord, TDescription, TAddress> OnCreateEndpointProvider<TRecord, TDescription, TAddress>()
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IEndpointProvider<TRecord, TDescription, TAddress>>(ServiceProvider);
        }

        public IEndpointProvider<TRecord, TDescription, TAddress> CreateEndpointProvider<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> endpointItemKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                OnCreateEndpointProvider<TRecord, TDescription, TAddress>();
        }
    }
}