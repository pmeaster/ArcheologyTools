using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordConverterFactory : IServiceFactory

    {
        IRecordConverter<TInput, TOutput> CreateRecordConverter<TInput, TOutput>(ServiceKey<TInput, TOutput> serviceKey = default)
            where TInput : class, IRecord
            where TOutput : class, IRecord;
    }
}