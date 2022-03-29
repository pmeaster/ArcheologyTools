using FractalSource.Services;

namespace FractalSource.Data
{
    public interface IRecordFactory : IServiceFactory
    {
        TRecord CreateRecord<TRecord>() where TRecord : IRecord;
    }
}