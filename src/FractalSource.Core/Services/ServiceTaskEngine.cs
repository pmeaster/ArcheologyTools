using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Configuration;
using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public class ServiceTaskEngine : ServiceEngine, INamedItem
    {
        private readonly IEnumerable<IServiceTask> _serviceTasks;

        public ServiceTaskEngine(ILoggerFactory loggerFactory, IEnumerable<IServiceTask> serviceTasks,
            IServiceTaskEngineConfiguration configuration)
            : base(loggerFactory)
        {
            _serviceTasks = serviceTasks;
            Name = configuration.ServiceName;
        }

        public string Name { get; set; }

        protected override async Task OnPreExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var serviceTask in _serviceTasks)
            {
                if (cancellationToken.IsCancellationRequested) break;

                await serviceTask.PreExecuteAsync(cancellationToken);
            }
        }

        protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var serviceTask in _serviceTasks)
            {
                if (cancellationToken.IsCancellationRequested) break;

                await serviceTask.ExecuteAsync(cancellationToken);
            }
        }

        protected override async Task OnPostExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var serviceTask in _serviceTasks)
            {
                if (cancellationToken.IsCancellationRequested) break;

                await serviceTask.PostExecuteAsync(cancellationToken);
            }
        }
    }
}