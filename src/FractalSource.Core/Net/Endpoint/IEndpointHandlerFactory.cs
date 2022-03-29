using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointHandlerFactory<in TConstraint> : IServiceFactory
        where TConstraint : class, IRecord
    {
        IEndpointHandler<TRecord> CreateEndpointHandler<TRecord>(ServiceKey<TRecord> serviceKey)
            where TRecord : class, TConstraint;
    }
}