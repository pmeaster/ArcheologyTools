using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordHandlerFactory : IServiceFactory
    {
        IRecordHandler<TInput, TOutput> CreateRecordHandler<TInput, TOutput>(ServiceKey<TInput, TOutput> serviceKey)
            where TInput : class, IRecord
            where TOutput : class, IRecord;
    }
}