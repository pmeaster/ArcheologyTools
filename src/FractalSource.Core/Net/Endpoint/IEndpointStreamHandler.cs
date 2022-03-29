using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointStreamHandler<TRecord, TDescription, TAddress> : IService<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>

    {
        Task<IEnumerable<TRecord>> HandleStreamAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}