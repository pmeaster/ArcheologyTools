using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

internal class LantisZoneLineHandler : Service<LocationEntity, MeasurementSystemEntity, LantisZoneEntity, Folder>, ILantisZoneLineHandler
{
    private const string LineColor = "80F2F0E6";
    private const string PolygonColor = "80F2F0E6";

    private readonly ILayoutPlacemarkHandler _layoutPlacemarkHandler;

    public LantisZoneLineHandler(ILayoutPlacemarkHandler layoutPlacemarkHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _layoutPlacemarkHandler = layoutPlacemarkHandler;
    }

    public Folder HandleLantisZoneLine(LocationEntity location, MeasurementSystemEntity measurementSystemEntity, LantisZoneEntity lantisZoneEntity)
    {
        return HandleLantisZoneLineAsync(location, measurementSystemEntity, lantisZoneEntity)
            .Result;
    }

    public async Task<Folder> HandleLantisZoneLineAsync(LocationEntity location, MeasurementSystemEntity measurementSystemEntity,
        LantisZoneEntity lantisZoneEntity)
    {
        var breadth
            = Math.Round((lantisZoneEntity.ZoneEnd - lantisZoneEntity.ZoneStart) / 1000, 2);
        var radius
            = Math.Round(lantisZoneEntity.ZoneEnd / 1000, 2);

        var formatParameters = new List<object>
        {
            Environment.NewLine,
            breadth,
            radius,
            measurementSystemEntity.StadiaAbbreviation
        };

        var folder = new Folder
        {
            Name = lantisZoneEntity.Name,
            Description = new Description
            {
                Text = string.Format(
                    "Breadth = {1} {3} (stadia).{0}Radius = {2} {3} (stadia).",
                    formatParameters.ToArray())
            }
        };

        if (lantisZoneEntity.ZoneType == LantisZoneType.Area)
        {
            var areaPlacemark = new KmlPlacemark
            {
                Name = folder.Name,
                Description = folder.Description.Text,
                PlacemarkType = KmlPlacemarkType.Polygon,
                Coordinates = location.Coordinates,
                Style =
                {
                    LabelSize = default,
                    Icon =
                    {
                        Size = default,
                        Url = default
                    }
                }
            };

            var areaGeometry = new KmlGeometry
            {
                Name = areaPlacemark.Name,
                Description = areaPlacemark.Description,
                Radii = new List<double>
                {
                    lantisZoneEntity.ZoneStart,
                    lantisZoneEntity.ZoneEnd
                },
                MeasurementSystemRatio = measurementSystemEntity.BaseRatio,
                LineStyle =
                {
                    Color = lantisZoneEntity.ZoneColor,
                    Width = 0
                },
                PolygonStyle =
                {
                    Color = lantisZoneEntity.ZoneColor,
                    Fill = default,
                    Outline = default
                }
            };

            folder.AddFeature(
                await _layoutPlacemarkHandler.HandlePlaceMarkAsync(areaPlacemark, areaGeometry)
                );
        }

        var linePlacemark = new KmlPlacemark
        {
            Name = folder.Name,
            Description = folder.Description.Text,
            PlacemarkType = KmlPlacemarkType.Line,
            Coordinates = location.Coordinates,
            Style =
            {
                LabelSize = default,
                Icon =
                {
                    Size = default,
                    Url = default
                }
            }
        };

        var lineGeometry = new KmlGeometry
        {
            Name = linePlacemark.Name,
            Description = linePlacemark.Description,
            Radii = new List<double>
            {
                lantisZoneEntity.ZoneEnd
            },
            MeasurementSystemRatio = measurementSystemEntity.BaseRatio,
            LineStyle =
            {
                Color = LineColor,
                Width = 1.5
            },
            PolygonStyle =
            {
                Color = PolygonColor,
                Fill = default,
                Outline = default
            }
        };

        folder.AddFeature(
            await _layoutPlacemarkHandler.HandlePlaceMarkAsync(linePlacemark, lineGeometry)
            );

        return folder;
    }
}