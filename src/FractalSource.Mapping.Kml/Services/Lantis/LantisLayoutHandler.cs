using FractalSource.Mapping.Services.Location;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Lantis;

internal class LantisLayoutHandler : LocationLayoutHandler, ILantisLayoutHandler
{
    public LantisLayoutHandler(ILocationPlaceMarkHandler placeMarkHandler,
        ILoggerFactory loggerFactory)
        : base(placeMarkHandler, loggerFactory)
    {
    }
}