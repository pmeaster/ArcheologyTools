using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public class LocationPlaceMarkHandler : Service<LocationEntity, KmlFeatureContainer>, ILocationPlaceMarkHandler
{
    private readonly ILayoutPlacemarkHandler _layoutPlacemarkHandler;

    public LocationPlaceMarkHandler(ILayoutPlacemarkHandler layoutPlacemarkHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _layoutPlacemarkHandler = layoutPlacemarkHandler;
    }

    public KmlFeatureContainer HandlePlaceMark(LocationEntity location, bool isAntipode = false)
    {
        return HandlePlaceMarkAsync(location, isAntipode)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandlePlaceMarkAsync(LocationEntity location, bool isAntipode = false)
    {
        var placemark
            = await HandlePlaceMarkAsyncImpl(location, isAntipode);

        return placemark.ToFeatureContainer();
    }

    private async Task<Placemark> HandlePlaceMarkAsyncImpl(LocationEntity location, bool isAntipode = false)
    {
        var coordinates = isAntipode
            ? location.Coordinates.GetAntipode()
            : location.Coordinates;

        var placemarkNameSuffix = isAntipode
            ? " (Antipode)"
            : string.Empty;

        var placemarkName = $"{location.Name}{placemarkNameSuffix}";
        var placemarkDescription = $"{location.Description ?? placemarkName}";

        var kmlPlacemark = new KmlPlacemark
        {
            Name = placemarkName,
            Description = placemarkDescription,
            PlacemarkType = KmlPlacemarkType.Point,
            Coordinates = coordinates,
            Style =
            {
                Name = default,
                LabelSize = LocationLayoutStyles.LabelSize,
                Icon =
                {
                    Size = LocationLayoutStyles.IconSize,
                    Url = LocationLayoutStyles.IconUrl
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