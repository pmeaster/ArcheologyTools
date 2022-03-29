using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpoint<TRecord, TDescription, TAddress> : IService<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        TAddress Address { get; }

        Task<IEnumerable<TRecord>> GetRecordsAsync(CancellationToken cancellationToken = default);
    }
}