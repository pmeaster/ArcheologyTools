using AutoMapper;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public class RecordConverter<TInput, TOutput> : Service, IRecordConverter<TInput, TOutput>
        where TInput : class, IRecord
        where TOutput : class, IRecord
    {
        private readonly IMapper _mapper;

        public RecordConverter(IMapper mapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _mapper = mapper;
        }

        protected virtual TOutput OnConvert(TInput inputRecord)
        {
            return _mapper.Map<TInput, TOutput>(inputRecord);
        }

        public TOutput Convert(TInput inputRecord)
        {
            return OnConvert(inputRecord);
        }
    }
}