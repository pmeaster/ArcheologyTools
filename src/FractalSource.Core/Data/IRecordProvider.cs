using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordProvider<TRecord> : IService
        where TRecord : class, IRecord
    {
        IEnumerable<TRecord> GetRecords();

        Task<IEnumerable<TRecord>> GetRecordsAsync(CancellationToken cancellationToken = default);
    }
}