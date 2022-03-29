using System;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Base;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Sites;

public class LayoutPlacemarkHandler : Service<KmlPlacemark, KmlGeometry, Placemark>, ILayoutPlacemarkHandler
{
    private readonly ILayoutGeometryHandler _layoutGeometryHandler;

    public LayoutPlacemarkHandler(ILayoutGeometryHandler layoutGeometryHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _layoutGeometryHandler = layoutGeometryHandler;
    }

    public Placemark HandlePlaceMark(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry)
    {
        return HandlePlaceMarkAsync(kmlPlacemark, kmlGeometry)
            .Result;
    }

    public async Task<Placemark> HandlePlaceMarkAsync(KmlPlacemark kmlPlacemark, KmlGeometry kmlGeometry)
    {
        var placemark = new Placemark
        {
            Name = kmlPlacemark.Name,
            Description = new Description
            {
                Text = kmlPlacemark.Description
            }
            //TODO: Determine default visibility of placemark items.
            //Visibility = DefaultStyles.FeatureVisibility
        };

        var placemarkStyle = new Style
        {
            Label = new LabelStyle
            {
                Scale = kmlPlacemark.Style.LabelSize
            },
            Line = new LineStyle
            {
                Color = string.IsNullOrWhiteSpace(kmlGeometry.LineStyle.Color)
                    ? default
                    : Color32.Parse(kmlGeometry.LineStyle.Color),
                Width = kmlGeometry.LineStyle.Width
            },
            Polygon = new PolygonStyle
            {
                Color = string.IsNullOrWhiteSpace(kmlGeometry.PolygonStyle.Color)
                    ? default
                    : Color32.Parse(kmlGeometry.PolygonStyle.Color),
                Fill = kmlGeometry.PolygonStyle.Fill,
                Outline = kmlGeometry.PolygonStyle.Outline
            }
        };

        if (!string.IsNullOrEmpty(kmlPlacemark.Style.Icon.Url))
        {
            placemarkStyle.Icon = new IconStyle
            {
                Icon = new IconStyle.IconLink(new Uri(kmlPlacemark.Style.Icon.Url)),
                Scale = kmlPlacemark.Style.Icon.Size
            };
        }

        placemark.AddStyle(placemarkStyle);

        placemark.Geometry
            = await _layoutGeometryHandler.HandleGeometryAsync(kmlPlacemark, placemark, kmlGeometry);

        return placemark;
    }
}