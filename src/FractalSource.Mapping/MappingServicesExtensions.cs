using FractalSource.Mapping.Geodesy;
using FractalSource.Mapping.Services.Geodesy;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Services;

public static class MappingServicesExtensions
{
    public static IServiceCollection AddFractalSourceMappingServices(this IServiceCollection services)
    {
        return services
            /* GeoCode Services */
            .AddTransient<IGeoCoordinatesFactory, GeoCoordinatesFactory>()
            .AddTransient<IGeodeticCalculatorFactory, GeodeticCalculatorFactory>();
    }

}