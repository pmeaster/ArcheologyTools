using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

// ReSharper disable LoopCanBeConvertedToQuery

namespace FractalSource.Data
{
    public class RecordHandler<TInput, TOutput> : Service<TInput, TOutput>, IRecordHandler<TInput, TOutput>
        where TInput : class, IRecord
        where TOutput : class, IRecord
    {
        private readonly IRecordConverter<TInput, TOutput> _recordConverter;
        private readonly IEnumerable<IRecordFilter<TInput>> _recordFilters;

        public RecordHandler(ILoggerFactory loggerFactory, IRecordConverterFactory recordConverterFactory, IEnumerable<IRecordFilter<TInput>> recordFilters)
            : base(loggerFactory)
        {
            _recordFilters = recordFilters;
            _recordConverter = recordConverterFactory.CreateRecordConverter(ServiceKeys.RecordConverterKey);
        }

        protected virtual async Task<IEnumerable<TOutput>> OnHandleRecordsAsync(IEnumerable<TInput> inputRecords, CancellationToken cancellationToken = default)
        {
            return await Task.Factory
                .StartNew(() => inputRecords.Select(input => _recordConverter.Convert(input)), cancellationToken);
        }

        public async Task<IEnumerable<TOutput>> HandleRecordsAsync(IEnumerable<TInput> inputRecords, CancellationToken cancellationToken = default)
        {
            IEnumerable<TInput> filteredRecords = inputRecords.ToList();

            foreach (var filter in _recordFilters)
            {
                filteredRecords = (await filter.FilterRecordsAsync(filteredRecords, cancellationToken)).ToList();
            }

            return
                await OnHandleRecordsAsync(filteredRecords, cancellationToken);
        }
    }
}