using FractalSource.Mapping.Data.Context;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Data.Services;

internal class ArcheologyContextFactory : ServiceFactory, IArcheologyContextFactory
{
    public ArcheologyContextFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        : base(serviceProvider, loggerFactory)
    {
    }

    public ArcheologyContext CreateContext()
    {
        return
            ActivatorUtilities
                .GetServiceOrCreateInstance<ArcheologyContext>(ServiceProvider);
    }
}