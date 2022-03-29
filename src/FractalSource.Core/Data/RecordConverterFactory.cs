using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public class RecordConverterFactory : ServiceFactory, IRecordConverterFactory
    {
        public RecordConverterFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected virtual IRecordConverter<TInput, TOutput> OnCreateRecordConverter<TInput, TOutput>()
            where TInput : class, IRecord
            where TOutput : class, IRecord
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IRecordConverter<TInput, TOutput>>(ServiceProvider);
        }

        public IRecordConverter<TInput, TOutput> CreateRecordConverter<TInput, TOutput>(ServiceKey<TInput, TOutput> serviceKey)
            where TInput : class, IRecord
            where TOutput : class, IRecord
        {
            return OnCreateRecordConverter<TInput, TOutput>();
        }
    }
}