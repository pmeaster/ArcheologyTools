using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordFilter<TRecord> : IService<TRecord>
        where TRecord : class, IRecord
    {
        Task<IEnumerable<TRecord>> FilterRecordsAsync(IEnumerable<TRecord> inputRecords, CancellationToken cancellationToken = default);
    }
}