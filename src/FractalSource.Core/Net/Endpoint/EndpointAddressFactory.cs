using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public sealed class EndpointAddressFactory : ServiceFactory, IEndpointAddressFactory
    {
        public EndpointAddressFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        private TAddress OnCreateAddress<TDescription, TAddress>()
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<TAddress>(ServiceProvider);
        }

        public TAddress CreateAddress<TDescription, TAddress>(ServiceKey<TDescription, TAddress> serviceKey = default)
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return OnCreateAddress<TDescription, TAddress>();
        }
    }
}