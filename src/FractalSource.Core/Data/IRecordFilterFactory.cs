using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordFilterFactory : IServiceFactory
    {
        IRecordFilter<TRecord> CreateRecordFilter<TRecord>(ServiceKey<TRecord> serviceKey)
            where TRecord : class, IRecord;
    }
}