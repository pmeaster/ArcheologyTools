using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public abstract class RecordFilter<TRecord> : Service<TRecord>, IRecordFilter<TRecord>
        where TRecord : class, IRecord
    {
        protected RecordFilter(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        protected abstract Task<IEnumerable<TRecord>> OnFilterRecordsAsync(IEnumerable<TRecord> inputRecords, CancellationToken cancellationToken = default);

        public async Task<IEnumerable<TRecord>> FilterRecordsAsync(IEnumerable<TRecord> inputRecords, CancellationToken cancellationToken = default)
        {
            return
                await OnFilterRecordsAsync(inputRecords, cancellationToken);
        }
    }
}