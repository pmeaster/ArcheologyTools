using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointStreamHandler<TRecord, TDescription, TAddress> : Service<TRecord, TDescription, TAddress>, IEndpointStreamHandler<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        protected EndpointStreamHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        protected abstract Task<IEnumerable<TRecord>> OnHandleStreamAsync(Stream stream, CancellationToken cancellationToken = default);

        public async Task<IEnumerable<TRecord>> HandleStreamAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            return
                await OnHandleStreamAsync(stream, cancellationToken);
        }
    }
}