using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Mapping.Services.Keyhole;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Mapping.Services.Sites;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Services;

public static class KmlServicesExtensions
{
    public static IServiceCollection AddFractalSourceMappingKmlServices(this IServiceCollection services)
    {
        return services
            /* Core Kml Services */
            .AddTransient<IKmlOutputHandler, KmlOutputHandler>()

            /* Location Services */
            .AddTransient<ILocationHandler, LocationHandler>()
            .AddTransient<ILocationLayoutHandler, LocationLayoutHandler>()
            .AddTransient<ILocationPlaceMarkHandler, LocationPlaceMarkHandler>()
            .AddTransient<ILocationWebLayoutHandler, LocationWebLayoutHandler>()
            .AddTransient<ILocationsWebHandler, LocationsWebHandler>()
            .AddTransient<ILocationWebHandler, LocationWebHandler>()

            /* Pole Location Services */
            .AddTransient<IPoleWebLayoutHandler, PoleWebLayoutHandler>()
            .AddTransient<IPolesTaskHandler, PolesTaskHandler>()
            .AddTransient<IPoleGridLayoutHandler, PoleGridLayoutHandler>()
            .AddTransient<IPoleLayoutHandler, PoleLayoutHandler>()
            .AddTransient<IPolePlaceMarkHandler, PolePlaceMarkHandler>()
            .AddTransient<IPoleGridLineHandler, PoleGridLineHandler>()
            .AddTransient<IPoleEquatorHandler, PoleEquatorHandler>()
            .AddTransient<IPoleParallelsHandler, PoleParallelsHandler>()
            .AddTransient<IPoleMeridiansHandler, PoleMeridiansHandler>()

            /* Site Location Services */
            .AddTransient<ISiteWebLayoutHandler, SiteWebLayoutHandler>()
            .AddTransient<ISiteLayoutHandler, SiteLayoutHandler>()
            .AddTransient<ISiteAlignmentsWebHandler, SiteAlignmentsWebHandler>()
            .AddTransient<ISiteAlignmentsHandler, SiteAlignmentsHandler>()

            /* Lantis Location Services */
            .AddTransient<ILantisWebLayoutHandler, LantisWebLayoutHandler>()
            .AddTransient<ILantisZonesWebLayoutHandler, LantisZonesWebLayoutHandler>()
            .AddTransient<ILantisLayoutHandler, LantisLayoutHandler>()
            .AddTransient<ILantisZonesMeasurementSystemLayoutHandler, 
                LantisZonesMeasurementSystemLayoutHandler>()
            .AddTransient<ILantisZoneLineHandler, LantisZoneLineHandler>()
            .AddTransient<ILantisZonesLayoutHandler, LantisZonesLayoutHandler>()

            /* Astronomy/Solar System Services */
            .AddTransient<ISolarSystemsWebLayoutHandler, SolarSystemsWebLayoutHandler>()
            .AddTransient<ISolarSystemLayoutMeasurementSystemHandler, SolarSystemLayoutMeasurementSystemHandler>()
            .AddTransient<ISolarSystemLayoutHandler, SolarSystemLayoutHandler>()
            //.AddTransient<ISolarSystemOrbitLayoutHandler, SolarSystemOrbitLayoutHandler>()
            .AddTransient<ISolarSystemOrbitBoundaryHandler, SolarSystemOrbitBoundaryHandler>()
            .AddTransient<ISolarSystemsLayoutHandler, SolarSystemsLayoutHandler>()

            /* Kml Geometry Services */
            .AddTransient<IPointGeometryHandler, PointGeometryHandler>()
            .AddTransient<ILineGeometryHandler, LineGeometryHandler>()
            .AddTransient<IPolygonGeometryHandler, PolygonGeometryHandler>()
            .AddTransient<IEllipseGeometryHandler, EllipseGeometryHandler>()
            .AddTransient<IArcGeometryHandler, ArcGeometryHandler>()
            .AddTransient<ISphereGeometryHandler, SphereGeometryHandler>()

            /* Layout Services */
            .AddTransient<ILayoutPlacemarkHandler, LayoutPlacemarkHandler>()
            .AddTransient<ILayoutGeometryHandler, LayoutGeometryHandler>();
    }

}