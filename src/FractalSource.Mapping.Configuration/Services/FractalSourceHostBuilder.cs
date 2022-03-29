using FractalSource.Mapping.Services;
using FractalSource.Services;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class FractalSourceHostBuilder
    {
        public static IHost Build(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services
                        .ConfigureFractalSourceServices();
                });

            var host = builder.Build();

            HostEnvironment.CurrentHost = host;

            return host;
        }

    }
}