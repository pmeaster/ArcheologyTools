using System;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public class EndpointFactory : ServiceFactory, IEndpointFactory
    {
        public EndpointFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected virtual IEndpoint<TRecord, TDescription, TAddress> OnCreateEndpoint<TRecord, TDescription, TAddress>()
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IEndpoint<TRecord, TDescription, TAddress>>(ServiceProvider);
        }

        public IEndpoint<TRecord, TDescription, TAddress> CreateEndpoint<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> endpointKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return OnCreateEndpoint<TRecord, TDescription, TAddress>();
        }
    }
}