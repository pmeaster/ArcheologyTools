using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointAddressFactory : IServiceFactory
    {
        TAddress CreateAddress<TDescription, TAddress>(ServiceKey<TDescription, TAddress> serviceKey = default)
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>;
    }
}