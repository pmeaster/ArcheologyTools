using System;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Data
{
    public abstract class RecordFilterFactory : ServiceFactory, IRecordFilterFactory
    {
        protected RecordFilterFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        protected abstract IRecordFilter<TRecord> OnCreateRecordFilter<TRecord>()
            where TRecord : class, IRecord;

        public IRecordFilter<TRecord> CreateRecordFilter<TRecord>(ServiceKey<TRecord> serviceKey)
            where TRecord : class, IRecord
        {
            return
                OnCreateRecordFilter<TRecord>();
        }
    }
}