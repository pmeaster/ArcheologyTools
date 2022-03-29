using System.Collections.Generic;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointServiceFactory : IServiceFactory
    {
        IEnumerable<IEndpointService> CreateEndpointServices();

        IEndpointService<TItem1, TItem2, TItem3, TItem4> CreateEndpointService<TItem1, TItem2, TItem3, TItem4>(ServiceKey<TItem1, TItem2, TItem3, TItem4> serviceKey = default);
    }
}