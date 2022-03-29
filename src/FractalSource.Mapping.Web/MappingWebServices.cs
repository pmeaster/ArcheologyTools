using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Web.Services.Providers;

// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Services;

public static class MappingWebServices
{
    public static IServiceCollection AddFractalSourceMappingWebServices(this IServiceCollection services)
    {
        return services
            /* Lantis Services */
            .AddTransient<ILantisZonesNetworkLinkProvider, LantisZonesNetworkLinkProvider>()
            .AddTransient<ILantisMeasurementSystemNetworkLinkProvider, LantisMeasurementSystemNetworkLinkProvider>()

            /* Solar System Services */
            .AddTransient<ISolarSystemsNetworkLinkProvider, SolarSystemsNetworkLinkProvider>()
            .AddTransient<ISolarSystemMeasurementSystemNetworkLinkProvider, SolarSystemMeasurementSystemNetworkLinkProvider>()

            /* Location Services */
            .AddTransient<ILocationNetworkLinkProvider, LocationNetworkLinkProvider>();
    }

}