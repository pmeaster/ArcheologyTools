using System.Threading;
using System.Threading.Tasks;

namespace FractalSource.Services
{
    public interface IServiceTask : IServiceItem
    {
        Task PreExecuteAsync(CancellationToken cancellationToken = default);

        Task ExecuteAsync(CancellationToken cancellationToken = default);

        Task PostExecuteAsync(CancellationToken cancellationToken = default);
    }
}