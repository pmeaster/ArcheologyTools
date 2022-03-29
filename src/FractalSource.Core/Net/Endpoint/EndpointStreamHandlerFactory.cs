using System;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public class EndpointStreamHandlerFactory : ServiceFactory, IEndpointStreamHandlerFactory
    {
        public EndpointStreamHandlerFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected virtual IEndpointStreamHandler<TRecord, TDescription, TAddress> OnCreateStreamHandler<TRecord, TDescription, TAddress>()
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>

        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IEndpointStreamHandler<TRecord, TDescription, TAddress>>(ServiceProvider);
        }


        public IEndpointStreamHandler<TRecord, TDescription, TAddress> CreateStreamHandler<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> serviceKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>

        {
            return
                OnCreateStreamHandler<TRecord, TDescription, TAddress>();
        }
    }
}