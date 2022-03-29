using FractalSource.Mapping.Configuration;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Services;

public static class ConfigurationServicesExtensions
{
    public static IServiceCollection ConfigureFractalSourceServices(this IServiceCollection services)
    {
        return services
            .AddFractalSourceCommonServices()
            .AddFractalSourceMappingServices()
            .AddFractalSourceMappingKmlServices()
            .AddFractalSourceMappingSqlDataServices()
            .AddFractalSourceMappingConfigurationServices();
    }

    public static IServiceCollection AddFractalSourceMappingConfigurationServices(this IServiceCollection services)
    {
        return services

            /* Sql Data Services Configuration */
            .AddTransient<ISqlDataConfiguration, SqlDataConfiguration>()

            /* Layout Services Configuration */
            .AddTransient<IKmlConfiguration, KmlConfiguration>();
    }

}