using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public sealed class EndpointDescriptionFactory : ServiceFactory, IEndpointDescriptionFactory
    {
        public EndpointDescriptionFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        private TDescription OnCreateDescription<TDescription>()
            where TDescription : class, IEndpointDescription
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<TDescription>(ServiceProvider);
        }

        public TDescription CreateDescription<TDescription>(ServiceKey<TDescription> serviceKey = default)
            where TDescription : class, IEndpointDescription
        {
            return OnCreateDescription<TDescription>();
        }
    }
}