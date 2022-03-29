using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointHandler<in TRecord> : IServiceItem
        where TRecord : class, IRecord
    {
        Task HandleEndpointAsync(IEnumerable<TRecord> outputRecords, CancellationToken cancellationToken = default);
    }
}