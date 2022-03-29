using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointProviderFactory : IServiceFactory
    {
        IEndpointProvider<TRecord, TDescription, TAddress> CreateEndpointProvider<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> endpointItemKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>;
    }
}