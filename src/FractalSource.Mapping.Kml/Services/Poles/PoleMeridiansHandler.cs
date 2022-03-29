using System;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

// ReSharper disable RedundantArgumentDefaultValue

namespace FractalSource.Mapping.Services.Poles;

internal class PoleMeridiansHandler : Service<LocationEntity>, IPoleMeridiansHandler
{
    private readonly IPoleGridLineHandler _poleGridLineHandler;

    public PoleMeridiansHandler(IPoleGridLineHandler poleGridLineHandler, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _poleGridLineHandler = poleGridLineHandler;
    }

    public KmlFeatureContainer HandlePoleLocationMeridians(LocationEntity location)
    {
        return HandlePoleLocationMeridiansAsync(location)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandlePoleLocationMeridiansAsync(LocationEntity location)
    {
        var folder = new Folder
        {
            Name = "Pole Meridian Lines"
        };

        await HandlePoleMeridiansAsync(location, folder);

        return folder.ToFeatureContainer();
    }

    private async Task HandlePoleMeridiansAsync(LocationEntity location, Folder parentFolder)
    {
        var equatorPlacemark
            = await _poleGridLineHandler.HandlePoleGridLineAsync(
                location,
                PoleParallels.GreatCircle,
                PoleKmlStyles.MeridianLineName,
                PoleKmlStyles.ParallelLineWidth);

        var equatorCoordinates
            = (equatorPlacemark.Geometry as LineString)?.Coordinates ?? new CoordinateCollection();

        var maxSkip = equatorCoordinates.Count - 1;

        var meridianName = $"{location.Name} {PoleKmlStyles.MeridianLineName}";

        var sidePoleLocation = new PoleLocationEntity
        {
            Name = meridianName,
            Description = meridianName,
            LineColor = location.LineColor
        };

        var increment = (int)Math.Round(equatorCoordinates.Count / 16.0, 0);
        var skip = 0;

        for (var i = 0; i < 8; i++)
        {
            sidePoleLocation.Coordinates = equatorCoordinates
                .Skip(Math.Min(skip, maxSkip))
                .FirstOrDefault()
                .ToGeoCoordinates();

            parentFolder.AddFeature(
                await _poleGridLineHandler.HandlePoleGridLineAsync(
                    sidePoleLocation,
                    PoleParallels.GreatCircle,
                    meridianName,
                    PoleKmlStyles.MeridianLineWidth)
            );

            skip += increment;
        }
    }
}