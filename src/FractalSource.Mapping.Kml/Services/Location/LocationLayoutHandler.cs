using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public class LocationLayoutHandler : Service<LocationEntity, KmlFeatureContainer>, ILocationLayoutHandler
{
    private readonly ILocationPlaceMarkHandler _placeMarkHandler;

    public LocationLayoutHandler(ILocationPlaceMarkHandler placeMarkHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _placeMarkHandler = placeMarkHandler;
    }

    public KmlFeatureContainer HandleLayout(LocationEntity location)
    {
        return OnHandleLayout(location);
    }

    public async Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location)
    {
        return 
            await OnHandleLayoutAsync(location);
    }

    protected virtual KmlFeatureContainer OnHandleLayout(LocationEntity location)
    {
        return OnHandleLayoutAsync(location)
            .Result;
    }

    protected virtual async Task<KmlFeatureContainer> OnHandleLayoutAsync(LocationEntity location)
    {
        var folder = new Folder
        {
            Name = location.Name,
            Description = new Description
            {
                Text = location.Description
            }
        };


        folder.AddFeature(
            (await _placeMarkHandler.HandlePlaceMarkAsync(location))
            .ToFeature()
        );

        return folder
            .ToFeatureContainer();
    }
}