using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordConverter<in TInput, out TOutput> : IService
        where TInput : class, IRecord
        where TOutput : class, IRecord
    {
        TOutput Convert(TInput inputRecord);
    }
}