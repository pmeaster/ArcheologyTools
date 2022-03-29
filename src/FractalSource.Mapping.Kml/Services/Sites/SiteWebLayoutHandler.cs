using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.Poles;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Sites;

internal class SiteWebLayoutHandler : LocationWebLayoutHandler, ISiteWebLayoutHandler
{
    public SiteWebLayoutHandler(ILocationLayoutHandler locationLayoutHandler,
        ILantisZonesWebLayoutHandler lantisZonesWebLayoutHandler,
        ISolarSystemsWebLayoutHandler solarSystemsWebLayoutHandler,
        IPoleGridLayoutHandler poleGridLayoutHandler,
        ILocationNetworkLinkProvider locationNetworkLinkProvider,
        ILoggerFactory loggerFactory)
        : base(locationLayoutHandler, lantisZonesWebLayoutHandler, solarSystemsWebLayoutHandler,
            poleGridLayoutHandler, locationNetworkLinkProvider, loggerFactory)
    {
    }
}