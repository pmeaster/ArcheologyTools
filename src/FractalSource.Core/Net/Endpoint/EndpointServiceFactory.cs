using System;
using System.Collections.Generic;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public class EndpointServiceFactory : ServiceFactory, IEndpointServiceFactory
    {
        private readonly IEnumerable<IEndpointService> _endpointServices;

        public EndpointServiceFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, 
            IEnumerable<IEndpointService> endpointServices)
            : base(serviceProvider, loggerFactory)
        {
            _endpointServices = endpointServices;
        }

        protected virtual IEnumerable<IEndpointService> OnCreateEndpointServices()
        {
            return _endpointServices;
        }

        public IEnumerable<IEndpointService> CreateEndpointServices()
        {
            return
                OnCreateEndpointServices();
        }

        public IEndpointService<TItem1, TItem2, TItem3, TItem4> CreateEndpointService<TItem1, TItem2, TItem3, TItem4>(ServiceKey<TItem1, TItem2, TItem3, TItem4> serviceKey = default)
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IEndpointService<TItem1, TItem2, TItem3, TItem4>>(ServiceProvider);
        }
    }
}