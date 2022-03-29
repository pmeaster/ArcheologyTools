using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointHandler<TRecord> : ServiceItem, IEndpointHandler<TRecord>
        where TRecord : class, IRecord
    {
        protected abstract Task OnHandleEndpointAsync(IEnumerable<TRecord> outputRecords, CancellationToken cancellationToken = default);

        public async Task HandleEndpointAsync(IEnumerable<TRecord> outputRecords, CancellationToken cancellationToken = default)
        {
            await OnHandleEndpointAsync(outputRecords, cancellationToken);
        }
    }
}