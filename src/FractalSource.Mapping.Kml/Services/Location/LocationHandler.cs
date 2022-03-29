using System.Threading.Tasks;
using FractalSource.Mapping.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

internal class LocationHandler : Service<LocationEntity, KmlFeatureContainer>, ILocationHandler
{
    private readonly IPoleLayoutHandler _poleLayoutHandler;
    private readonly ILantisZonesLayoutHandler _lantisZonesLayoutHandler;
    private readonly ISolarSystemsLayoutHandler _solarSystemsLayoutHandler;
    //private readonly ILocationCompassLayoutHandler _locationCompassLayoutHandler;

    public LocationHandler(IPoleLayoutHandler poleLayoutHandler, ILantisZonesLayoutHandler lantisZonesLayoutHandler,
       ISolarSystemsLayoutHandler solarSystemsLayoutHandler, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _poleLayoutHandler = poleLayoutHandler;
        _lantisZonesLayoutHandler = lantisZonesLayoutHandler;
        _solarSystemsLayoutHandler = solarSystemsLayoutHandler;
    }

    public KmlFeatureContainer HandleLocation(LocationEntity location)
    {
        return HandleLocationAsync(location)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandleLocationAsync(LocationEntity location)
    {
        var folder = await HandleLocationTypesAsync(location);

        //if (string.IsNullOrWhiteSpace(location.LineColor))
        //{
        //    location.LineColor
        //        = ColorGenerator.GetRandomColor(ColorBrightnessLevel.High);
        //}

        var antipodeFeature = await HandleLocationTypesAsync(location.GetAntipode(), true);

        folder.AddFeature(antipodeFeature);

        return folder.ToFeatureContainer();
    }

    private async Task<Folder> HandleLocationTypesAsync(LocationEntity location, bool isAntipode = false)
    {
        var folder = new Folder
        {
            Name = location.Name,
            Description = new Description
            {
                Text = location.Description
            }
        };

        if (!isAntipode)
        {
            //Pole Layout includes the antipode (South Pole)
            var poleFeature
                = await _poleLayoutHandler.HandleLayoutAsync(location);

            folder.AddFeature(
                poleFeature.ToFeature());
        }

        var lantisZoneLayoutFeature
            = await _lantisZonesLayoutHandler.HandleLayoutAsync(location);

        folder.AddFeature(
            lantisZoneLayoutFeature.ToFeature());

        var solarSystemFeature
            = await _solarSystemsLayoutHandler.HandleLayoutAsync(location);

        folder.AddFeature(
            solarSystemFeature.ToFeature());

        //var compassFeature
        //    = await _locationCompassLayoutHandler.HandleLocationCompassLayoutAsync(location);

        //folder.AddFeature(
        //    compassFeature.ToFeature());

        return folder;
    }
}