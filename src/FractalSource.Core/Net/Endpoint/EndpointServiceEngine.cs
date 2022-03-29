using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Configuration;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    //TODO: handle task cancelled exceptions 
    public class EndpointServiceEngine : ServiceEngine
    {
        private readonly IEnumerable<IServiceTask> _serviceEngineTasks;
        private readonly IEnumerable<IEndpointService> _endpointServices;
        private readonly IServiceEngineConfiguration _configuration;

        public EndpointServiceEngine(ILoggerFactory loggerFactory, IEndpointServiceFactory endpointServiceFactory,
            IEnumerable<IServiceTask> serviceEngineTasks, IServiceEngineConfiguration configuration)
            : base(loggerFactory)
        {
            _serviceEngineTasks = serviceEngineTasks;
            _configuration = configuration;
            _endpointServices = endpointServiceFactory.CreateEndpointServices();
        }

        protected override async Task OnPreExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var serviceTask in _serviceEngineTasks)
            {
                if (cancellationToken.IsCancellationRequested) break;

                await serviceTask.PreExecuteAsync(cancellationToken);
            }
        }

        protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
        {
            var delay = _configuration.EndpointEnumerationDelay;

            var i = 0;
            foreach (var endpointService in _endpointServices)
            {
                i++;

                if (cancellationToken.IsCancellationRequested) break;

                await endpointService.ExecuteEndpointAsync(cancellationToken);

                if (i == _endpointServices.Count()) continue;

                Logger
                    .LogInformation(delay > 0
                        ? $"Endpoint processed.{Environment.NewLine}Pausing for {delay} second{(delay > 1 ? "s" : string.Empty)} before executing next endpoint..."
                        : $"Endpoint processed.{Environment.NewLine}");

                await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
            }
        }

        protected override async Task OnPostExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var serviceTask in _serviceEngineTasks)
            {
                if (cancellationToken.IsCancellationRequested) break;

                await serviceTask.PostExecuteAsync(cancellationToken);
            }
        }
    }
}