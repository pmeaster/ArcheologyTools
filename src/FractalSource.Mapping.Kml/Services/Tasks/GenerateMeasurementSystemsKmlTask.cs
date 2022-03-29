using System;
using System.Threading;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Tasks
{
    public class GenerateMeasurementSystemsKmlTask : ServiceTask
    {
        private readonly IKmlConfiguration _kmlConfiguration;
        private readonly IKmlOutputHandler _kmlOutputHandler;

        public GenerateMeasurementSystemsKmlTask(IKmlConfiguration kmlConfiguration, 
            IKmlOutputHandler kmlOutputHandler, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _kmlConfiguration = kmlConfiguration;
            _kmlOutputHandler = kmlOutputHandler;
        }

        protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
        {
            Logger
                .LogMethodStart(nameof(Task.CompletedTask));

            try
            {
                await Task.CompletedTask;
                //var kmlFiles = _kmlConfiguration.Layouts
                //    .Select(layout => _layoutHandlerOld.HandleLayout(layout)).ToList();

                //await _kmlOutputHandler.HandleKmlOutputAsync(kmlFiles, "Systems");
            }
            catch (Exception e)
            {
                Logger
                    .LogError(e, "An unhandled exception occurred.");

                throw;
            }

            Logger
                .LogMethodEnd(nameof(Task.CompletedTask));
        }
    }
}