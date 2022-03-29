using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleGridLineHandler : Service<LocationEntity, Placemark>, IPoleGridLineHandler
{
    private readonly ILayoutPlacemarkHandler _layoutPlacemarkHandler;

    public PoleGridLineHandler(ILayoutPlacemarkHandler layoutPlacemarkHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _layoutPlacemarkHandler = layoutPlacemarkHandler;
    }

    public Placemark HandlePoleGridLine(LocationEntity location, double radius,
        string gridLineName = PoleKmlStyles.GridLineName,
        double lineWidth = PoleKmlStyles.GridLineWidth)
    {
        return HandlePoleGridLineAsync(location, radius, gridLineName, lineWidth)
            .Result;
    }

    public async Task<Placemark> HandlePoleGridLineAsync(LocationEntity location, double radius,
        string gridLineName = PoleKmlStyles.GridLineName,
        double lineWidth = PoleKmlStyles.GridLineWidth)
    {
        var placemarkName = $"{gridLineName} ({location.Name} Pole Grid Line)";

        var kmlPlacemark = new KmlPlacemark
        {
            Name = placemarkName,
            Description = placemarkName,
            PlacemarkType = KmlPlacemarkType.Line,
            Coordinates = location.Coordinates,
            Style =
            {
                //This is set this way to use as a template
                Name = default,
                LabelSize = default,
                Icon =
                {
                    Size = default,
                    Url = default
                }
            }
        };

        var kmlGeometry = new KmlGeometry
        {
            Name = kmlPlacemark.Name,
            Description = kmlPlacemark.Description,
            Radii = new List<double>
            {
                radius
            },
            MeasurementSystemRatio = 1,
            LineStyle =
            {
                Color = location.LineColor,
                Width = lineWidth
            },
            PolygonStyle =
            {
                //This is set this way to use as a template
                Color = default,
                Fill = default,
                Outline = default
            }
        };

        var placemark = await _layoutPlacemarkHandler.HandlePlaceMarkAsync(kmlPlacemark, kmlGeometry);

        return placemark;
    }
}