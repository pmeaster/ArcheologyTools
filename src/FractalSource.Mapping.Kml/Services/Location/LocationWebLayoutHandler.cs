using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public class LocationWebLayoutHandler : Service<LocationEntity, Feature>, ILocationWebLayoutHandler
{
    private readonly ILocationLayoutHandler _locationLayoutHandler;
    private readonly ILantisZonesWebLayoutHandler _lantisZonesWebLayoutHandler;
    private readonly ISolarSystemsWebLayoutHandler _solarSystemsWebLayoutHandler;
    private readonly IPoleGridLayoutHandler _poleGridLayoutHandler;
    private readonly ILocationNetworkLinkProvider _locationNetworkLinkProvider;

    public LocationWebLayoutHandler(ILocationLayoutHandler locationLayoutHandler,
        ILantisZonesWebLayoutHandler lantisZonesWebLayoutHandler,
        ISolarSystemsWebLayoutHandler solarSystemsWebLayoutHandler,
        IPoleGridLayoutHandler poleGridLayoutHandler,
        ILocationNetworkLinkProvider locationNetworkLinkProvider,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationLayoutHandler = locationLayoutHandler;
        _lantisZonesWebLayoutHandler = lantisZonesWebLayoutHandler;
        _solarSystemsWebLayoutHandler = solarSystemsWebLayoutHandler;
        _poleGridLayoutHandler = poleGridLayoutHandler;
        _locationNetworkLinkProvider = locationNetworkLinkProvider;
    }

    public Feature HandleLayout(LocationEntity location, bool includeAntipode = true)
    {
        return HandleLayoutAsync(location, includeAntipode)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location, bool includeAntipode = true)
    {
        var container = await _locationLayoutHandler.HandleLayoutAsync(location);

        var feature = container.ToFeature();

        var folder = (feature as Container) ??
                     new Folder
                     {
                         Name = feature.Name,
                         Description = feature.Description
                     };

        if (feature is not Container)
        {
            folder.AddFeature(feature);
        }

        var layoutsFolder = folder.AddFolder("Layouts");

        var poleGridFeature
            = await _poleGridLayoutHandler.HandleLayoutAsync(location);

        (poleGridFeature as Container)?.MarkVisibilityRecursive(false);

        layoutsFolder.AddFeature(poleGridFeature);

        layoutsFolder.AddFeature(
            await _lantisZonesWebLayoutHandler.HandleLayoutAsync(location)
        );

        layoutsFolder.AddFeature(
            await _solarSystemsWebLayoutHandler.HandleLayoutAsync(location)
        );


        if (!includeAntipode)
        {
            return folder;
        }

        var antipodeNetworkLink
            = _locationNetworkLinkProvider.GetNetworkLink(location, true);

        antipodeNetworkLink.Name = "Antipode";
        antipodeNetworkLink.Description = null;

        folder.AddFeature(
            antipodeNetworkLink
        );

        return folder;
    }
}