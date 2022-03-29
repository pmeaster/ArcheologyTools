using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointStreamHandlerFactory : IServiceFactory
    {
        IEndpointStreamHandler<TRecord, TDescription, TAddress> CreateStreamHandler<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> serviceKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>;
    }
}