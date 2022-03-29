using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public abstract class ServiceTask : ServiceItem, IServiceTask
    {
        protected ServiceTask(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        protected virtual async Task OnPreExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnPostExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public async Task PreExecuteAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogMethodStart(nameof(OnPreExecuteAsync));
            await OnPreExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(OnPreExecuteAsync));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogMethodStart(nameof(ExecuteAsync));
            await OnExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(ExecuteAsync));
        }

        public async Task PostExecuteAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogMethodStart(nameof(OnPostExecuteAsync));
            await OnPostExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(OnPostExecuteAsync));
        }
    }
}