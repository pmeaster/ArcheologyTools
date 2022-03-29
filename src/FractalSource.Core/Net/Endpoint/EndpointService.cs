using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Data;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Net.Endpoint
{
    public class EndpointService<TInput, TOutput, TDescription, TAddress>
        : Service<TInput, TOutput, TDescription, TAddress>, IEndpointService<TInput, TOutput, TDescription, TAddress>
        where TInput : class, IRecord
        where TOutput : class, IRecord
        where TDescription : class, IEndpointDescription
        where TAddress : class, IEndpointAddress<TDescription>
    {
        private readonly IRecordHandler<TInput, TOutput> _recordHandler;
        private readonly IEndpointHandler<TOutput> _endpointHandler;
        private readonly IEndpointProvider<TInput, TDescription, TAddress> _endpointProvider;

        public EndpointService(ILoggerFactory loggerFactory, IEndpointHandlerFactory<TOutput> endpointHandlerFactory,
            IRecordHandlerFactory recordHandlerFactory, IEndpointProviderFactory endpointProviderFactory)
            : base(loggerFactory)
        {
            _recordHandler = recordHandlerFactory.CreateRecordHandler(ServiceKeys.RecordHandlerKey);
            _endpointHandler = endpointHandlerFactory.CreateEndpointHandler(ServiceKeys.EndpointHandlerKey);
            _endpointProvider = endpointProviderFactory.CreateEndpointProvider(ServiceKeys.EndpointProviderKey);
        }

        private async Task ExecuteEndpointAsyncInternal(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Processing {typeof(TDescription).Name} Endpoint.");

            var endpoints = await _endpointProvider.GetEndpointsAsync(cancellationToken);

            var tasks = endpoints.Select(endpoint => Task.Factory.StartNew(() =>
            {
                var inputRecords = endpoint.GetRecordsAsync(cancellationToken)
                    .Result;

                var outputRecords = _recordHandler.HandleRecordsAsync(inputRecords, cancellationToken)
                    .Result;

                Console.Write(".");

                return outputRecords;
            }, cancellationToken));

            var taskList = tasks as Task<IEnumerable<TOutput>>[] ?? tasks.ToArray();

            Task.WaitAll(taskList.Cast<Task>()
                .ToArray(), cancellationToken);

            Console.WriteLine();

            var allRecords = taskList.SelectMany(task => task.Result);

            await _endpointHandler.HandleEndpointAsync(allRecords, cancellationToken);
        }

        public async Task ExecuteEndpointAsync(CancellationToken cancellationToken = default)
        {
            await ExecuteEndpointAsyncInternal(cancellationToken);
        }
    }
}