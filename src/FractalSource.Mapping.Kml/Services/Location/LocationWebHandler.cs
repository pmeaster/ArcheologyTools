using System;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Mapping.Services.Sites;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public class LocationWebHandler : Service<LocationEntity, Feature>, ILocationWebHandler
{
    
    private readonly ISiteWebLayoutHandler _siteWebLayoutHandler;
    private readonly IPoleWebLayoutHandler _poleWebLayoutHandler;
    private readonly ILantisWebLayoutHandler _lantisWebLayoutHandler;
    private readonly ILocationNetworkLinkProvider _locationNetworkLinkProvider;

    public LocationWebHandler(ISiteWebLayoutHandler siteWebLayoutHandler, 
        IPoleWebLayoutHandler poleWebLayoutHandler,
        ILantisWebLayoutHandler lantisWebLayoutHandler, 
        ILocationNetworkLinkProvider locationNetworkLinkProvider,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _siteWebLayoutHandler = siteWebLayoutHandler;
        _poleWebLayoutHandler = poleWebLayoutHandler;
        _lantisWebLayoutHandler = lantisWebLayoutHandler;
        _locationNetworkLinkProvider = locationNetworkLinkProvider;
    }

    public Feature HandleLocation(LocationEntity location, bool useNetworkLinks = false, 
        bool includeAntipode = true)
    {
        return HandleLocationAsync(location, useNetworkLinks, includeAntipode)
            .Result;
    }

    public async Task<Feature> HandleLocationAsync(LocationEntity location, bool useNetworkLinks = false, 
        bool includeAntipode = true)
    {
        if (!useNetworkLinks)
        {
            switch (location.LocationType)
            {
                case LocationType.Lantis:
                    return await _lantisWebLayoutHandler.HandleLayoutAsync(location, includeAntipode);

                case LocationType.Pole:
                    return await _poleWebLayoutHandler.HandleLayoutAsync(location);

                case LocationType.Site:
                    return await _siteWebLayoutHandler.HandleLayoutAsync(location, includeAntipode);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return 
            _locationNetworkLinkProvider.GetNetworkLink(location);
            
    }
}