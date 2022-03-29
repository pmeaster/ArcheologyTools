using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointProvider<TRecord, TDescription, TAddress> : Service<TRecord, TDescription, TAddress>, IEndpointProvider<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        protected EndpointProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public async Task<IEnumerable<IEndpoint<TRecord, TDescription, TAddress>>> GetEndpointsAsync(CancellationToken cancellationToken = default)
        {
            return
                await OnGetEndpointsAsync(cancellationToken);
        }

        protected abstract Task<IEnumerable<IEndpoint<TRecord, TDescription, TAddress>>> OnGetEndpointsAsync(CancellationToken cancellationToken = default);
    }
}