using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleLayoutHandler : Service<LocationEntity, KmlFeatureContainer>, IPoleLayoutHandler
{
    private readonly IPolePlaceMarkHandler _polePlaceMarkHandler;
    private readonly IPoleGridLayoutHandler _poleGridLayoutHandler;

    public PoleLayoutHandler(IPolePlaceMarkHandler polePlaceMarkHandler, 
        IPoleGridLayoutHandler poleGridLayoutHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _polePlaceMarkHandler = polePlaceMarkHandler;
        _poleGridLayoutHandler = poleGridLayoutHandler;
    }

    public KmlFeatureContainer HandleLayout(LocationEntity location)
    {
        return HandleLayoutAsync(location)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location)
    {
        var folder = new Folder
        {
            Name = location.Name,
            Description = new Description
            {
                Text = location.Description
            }
        };

        //var placemarksFolder = folder.AddFolder("Pole Placemarks");

        folder.AddFeature(
            (await _polePlaceMarkHandler.HandlePlaceMarkAsync(location))
            .ToFeature()
        );

        folder.AddFeature(
            (await _polePlaceMarkHandler.HandlePlaceMarkAsync(location, false))
            .ToFeature()
        );

        var poleGridFeature
            = await _poleGridLayoutHandler.HandleLayoutAsync(location);

        (poleGridFeature as Container)?.MarkVisibilityRecursive(false);

        if (location.LocationType == LocationType.Pole)
        {
            poleGridFeature.Description = null;
        }

        folder.AddFeature(poleGridFeature);

        return folder
            .ToFeatureContainer();
    }
}