using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointRecordProvider<TRecord, TDescription, TAddress> 
        : Service<TRecord, TDescription, TAddress>, IEndpointRecordProvider<TRecord, TDescription, TAddress>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        protected EndpointRecordProvider(ILoggerFactory loggerFactory, IEndpointAddressFactory addressFactory)
            : base(loggerFactory)
        {
            Address = addressFactory.CreateAddress(ServiceKeys.AddressKey);
        }

        public TAddress Address { get; }

        protected abstract Task<IEnumerable<TRecord>> OnGetRecordsAsync(CancellationToken cancellationToken = default);

        public IEnumerable<TRecord> GetRecords()
        {
            return GetRecordsAsync()
                .Result;
        }

        public async Task<IEnumerable<TRecord>> GetRecordsAsync(CancellationToken cancellationToken = default)
        {
            return await OnGetRecordsAsync(cancellationToken);
        }
    }
}