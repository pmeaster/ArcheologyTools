using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordHandler<TInput, TOutput> : IService<TInput, TOutput>
        where TInput : class, IRecord
        where TOutput : class, IRecord
    {
        Task<IEnumerable<TOutput>> HandleRecordsAsync(IEnumerable<TInput> inputRecords, CancellationToken cancellationToken = default);
    }
}