using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointProvider<TRecord, TDescription, TAddress> : IService<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        Task<IEnumerable<IEndpoint<TRecord, TDescription, TAddress>>> GetEndpointsAsync(CancellationToken cancellationToken = default);
    }
}