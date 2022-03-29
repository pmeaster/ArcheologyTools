using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PolePlaceMarkHandler : Service<LocationEntity>, IPolePlaceMarkHandler
{
    private readonly ILayoutPlacemarkHandler _layoutPlacemarkHandler;

    public PolePlaceMarkHandler(ILayoutPlacemarkHandler layoutPlacemarkHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _layoutPlacemarkHandler = layoutPlacemarkHandler;
    }

    public KmlFeatureContainer HandlePlaceMark(LocationEntity location, bool isNorth = true)
    {
        return HandlePlaceMarkAsync(location, isNorth)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandlePlaceMarkAsync(LocationEntity location, bool isNorth = true)
    {
        var placemark 
            = await HandlePoleLocationPlaceMarkAsyncImpl(location, isNorth);

        return placemark.ToFeatureContainer();
    }

    private async Task<Placemark> HandlePoleLocationPlaceMarkAsyncImpl(LocationEntity location, bool isNorth = true)
    {
        var coordinates = isNorth
            ? location.Coordinates
            : location.Coordinates.GetAntipode();

        var poleDirection = isNorth
            ? "North"
            : "South";

        var poleName = $"{location.Name} {poleDirection} Pole";
        var poleDescription = $"{location.Description ?? poleName}";

        var kmlPlacemark = new KmlPlacemark
        {
            Name = poleName,
            Description = poleDescription,
            PlacemarkType = KmlPlacemarkType.Point,
            Coordinates = coordinates,
            Style =
            {
                //This is set this way to use as a template
                Name = default,
                LabelSize = PoleKmlStyles.LabelSize,
                Icon =
                {
                    Size = PoleKmlStyles.IconSize,
                    Url = PoleKmlStyles.IconUrl
                }
            }
        };

        var kmlGeometry = new KmlGeometry
        {
            LineStyle =
            {
                Color = location.LineColor,
                Width = default
            }
        };

        return
            await _layoutPlacemarkHandler.HandlePlaceMarkAsync(kmlPlacemark, kmlGeometry);
    }
}