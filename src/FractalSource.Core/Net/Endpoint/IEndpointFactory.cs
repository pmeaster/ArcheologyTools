using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointFactory : IServiceFactory
    {
        IEndpoint<TRecord, TDescription, TAddress> CreateEndpoint<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> endpointKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>;
    }
}