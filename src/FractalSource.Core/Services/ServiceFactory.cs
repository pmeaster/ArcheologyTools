using System;
using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public abstract class ServiceFactory : Service, IServiceFactory
    {
        protected ServiceFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ServiceProvider = serviceProvider;
        }

        protected IServiceProvider ServiceProvider { get; }
    }
}