using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public abstract class ServiceEngine : Service, IServiceEngine
    {
        private Task _executingTask;
        private CancellationTokenSource _stoppingCts;

        protected ServiceEngine(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogMethodStart(nameof(OnPreExecuteAsync));
            await OnPreExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(OnPreExecuteAsync));

            Logger.LogMethodStart(nameof(OnExecuteAsync));
            await OnExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(OnExecuteAsync));

            Logger.LogMethodStart(nameof(OnPostExecuteAsync));
            await OnPostExecuteAsync(cancellationToken);
            Logger.LogMethodEnd(nameof(OnPostExecuteAsync));
        }

        protected abstract Task OnPreExecuteAsync(CancellationToken cancellationToken = default);

        protected abstract Task OnExecuteAsync(CancellationToken cancellationToken = default);

        protected abstract Task OnPostExecuteAsync(CancellationToken cancellationToken = default);

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(new[]
            {
                cancellationToken
            });

            Logger.LogMethodStart(nameof(ExecuteAsync));
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            Logger.LogMethodEnd(nameof(ExecuteAsync));

            return
                _executingTask.IsCompleted
                    ? _executingTask
                    : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(new[]
                    {
                        _executingTask,

                        Task.Delay(-1, cancellationToken)
                    })
                    .ConfigureAwait(false);
            }
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();

            GC.SuppressFinalize(this);
        }
    }
}