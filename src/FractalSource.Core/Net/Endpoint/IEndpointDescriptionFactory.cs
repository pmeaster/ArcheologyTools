using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointDescriptionFactory : IServiceFactory
    {
        TDescription CreateDescription<TDescription>(ServiceKey<TDescription> serviceKey = default)
            where TDescription : class, IEndpointDescription;
    }
}