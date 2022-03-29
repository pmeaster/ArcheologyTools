using System.Threading;
using System.Threading.Tasks;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointService<TItem1, TItem2, TItem3, TItem4> : IEndpointService, IService<TItem1, TItem2, TItem3, TItem4>
    {
    }

    public interface IEndpointService : IService
    {
        Task ExecuteEndpointAsync(CancellationToken cancellationToken = default);
    }
}