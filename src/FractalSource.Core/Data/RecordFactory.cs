using System;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public sealed class RecordFactory : ServiceFactory, IRecordFactory
    {
        public RecordFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        public TRecord CreateRecord<TRecord>()
            where TRecord : IRecord
        {
            return ActivatorUtilities.GetServiceOrCreateInstance<TRecord>(ServiceProvider);
        }
    }
}