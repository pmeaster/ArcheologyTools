using FractalSource.Data;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointRecordProvider<TRecord, TDescription, TAddress> : IService<TRecord, TDescription, TAddress>, IRecordProvider<TRecord>
        where TRecord : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        TAddress Address { get; }
    }
}