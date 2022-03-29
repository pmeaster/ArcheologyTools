using System;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public sealed class EndpointRecordProviderFactory : ServiceFactory, IEndpointRecordProviderFactory
    {
        public EndpointRecordProviderFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        private IEndpointRecordProvider<TRecord, TDescription, TAddress> OnCreateRecordProvider<TRecord, TDescription, TAddress>()
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IEndpointRecordProvider<TRecord, TDescription, TAddress>>(ServiceProvider);
        }

        public IEndpointRecordProvider<TRecord, TDescription, TAddress> CreateRecordProvider<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> serviceKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>
        {
            return
                OnCreateRecordProvider<TRecord, TDescription, TAddress>();
        }
    }
}