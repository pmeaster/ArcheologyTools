using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointRecordProviderFactory : IServiceFactory
    {
        IEndpointRecordProvider<TRecord, TDescription, TAddress> CreateRecordProvider<TRecord, TDescription, TAddress>(ServiceKey<TRecord, TDescription, TAddress> serviceKey = default)
            where TRecord : class, IRecord
            where TDescription : class, IEndpointDescription
            where TAddress : class, IEndpointAddress<TDescription>;
    }
}