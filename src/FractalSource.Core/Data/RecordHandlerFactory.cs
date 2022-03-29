using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public class RecordHandlerFactory : ServiceFactory, IRecordHandlerFactory
    {
        public RecordHandlerFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected virtual IRecordHandler<TInput, TOutput> OnCreateRecordHandler<TInput, TOutput>()
            where TInput : class, IRecord
            where TOutput : class, IRecord
        {
            return
                ActivatorUtilities
                    .GetServiceOrCreateInstance<IRecordHandler<TInput, TOutput>>(ServiceProvider);
        }

        public IRecordHandler<TInput, TOutput> CreateRecordHandler<TInput, TOutput>(ServiceKey<TInput, TOutput> serviceKey)
            where TInput : class, IRecord
            where TOutput : class, IRecord
        {
            return
                OnCreateRecordHandler<TInput, TOutput>();
        }
    }
}