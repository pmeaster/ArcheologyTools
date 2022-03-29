using FractalSource.Mapping.Services.Location;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Sites;

internal class SiteLayoutHandler : LocationLayoutHandler, ISiteLayoutHandler
{
    public SiteLayoutHandler(ILocationPlaceMarkHandler placeMarkHandler,
        ILoggerFactory loggerFactory)
        : base(placeMarkHandler, loggerFactory)
    {
    }
}