using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Mapping.Services.Location;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Base;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Sites;

internal class SiteAlignmentsHandler : Service<SiteLocationEntity, KmlFeatureContainer>, ISiteAlignmentsHandler
{
    private const string NorthSouthLineColor = "8000ffff";
    private const string EastWestLineColor = "80ffaaff";
    private const double LineDistanceInMeters = 18000000D;

    private readonly IGeoCoordinatesFactory _geoCoordinatesFactory;
    private readonly ILocationPlaceMarkHandler _placeMarkHandler;

    public SiteAlignmentsHandler(ILocationPlaceMarkHandler placeMarkHandler,
        IGeoCoordinatesFactory geoCoordinatesFactory,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _placeMarkHandler = placeMarkHandler;
        _geoCoordinatesFactory = geoCoordinatesFactory;
    }

    public KmlFeatureContainer HandleSiteAlignments(SiteLocationEntity site, SiteAlignmentDirection alignmentDirection)
    {
        return OnHandleSiteAlignments(site, alignmentDirection);
    }

    public async Task<KmlFeatureContainer> HandleSiteAlignmentsAsync(SiteLocationEntity site, SiteAlignmentDirection alignmentDirection)
    {
        return
            await OnHandleSiteAlignmentsAsync(site, alignmentDirection);
    }

    protected virtual KmlFeatureContainer OnHandleSiteAlignments(SiteLocationEntity site, SiteAlignmentDirection alignmentDirection)
    {
        return OnHandleSiteAlignmentsAsync(site, alignmentDirection)
            .Result;
    }

    protected virtual async Task<KmlFeatureContainer> OnHandleSiteAlignmentsAsync(SiteLocationEntity site, SiteAlignmentDirection alignmentDirection)
    {
        var folder = new Folder
        {
            Name = site.Name,
            Description = new Description
            {
                Text = site.Description
            }
        };

        var placemark = (await _placeMarkHandler.HandlePlaceMarkAsync(site)).ToFeature();
        placemark.Visibility = false;

        folder.AddFeature(placemark);

        if (!site.North.HasValue || !site.East.HasValue)
        {
            return folder
                .ToFeatureContainer();
        }

        var alignmentAxisFolder = folder.AddFolder("Alignments");

        var siteCoordinates = site.Coordinates;

        if (alignmentDirection is SiteAlignmentDirection.All or SiteAlignmentDirection.NorthSouth)
        {
            var north = site.North.Value;

            var south = north >= 0
                ? north + 180
                : north - 180;

            alignmentAxisFolder.AddFeature(
                CreatePlacemark(siteCoordinates, new Angle(north), site.Name, alignmentDirection)
                );

            alignmentAxisFolder.AddFeature(
                CreatePlacemark(siteCoordinates, new Angle(south), site.Name, alignmentDirection)
                );
        }

        if (alignmentDirection is SiteAlignmentDirection.All or SiteAlignmentDirection.EastWest)
        {
            var east = site.East.Value;

            var west = east + 180;

            if (west >= 360)
            {
                west -= 360;
            }

            alignmentAxisFolder.AddFeature(
                CreatePlacemark(siteCoordinates, new Angle(east), site.Name, alignmentDirection)
            );

            alignmentAxisFolder.AddFeature(
                CreatePlacemark(siteCoordinates, new Angle(west), site.Name, alignmentDirection)
            );
        }

        return folder
            .ToFeatureContainer();
    }

    private Placemark CreatePlacemark(GeoCoordinates startCoordinates, Angle angle, string siteName, SiteAlignmentDirection alignmentDirection)
    {
        var endCoordinates =
            _geoCoordinatesFactory
                .CalculateEndingGeoCoordinates(startCoordinates, angle.Radians, LineDistanceInMeters);

        var geometry = new[]
        {
            startCoordinates,
            endCoordinates

        }.ToLine();

        var placemark = new Placemark
        {
            Name = siteName,
            Geometry = geometry,
            Visibility = false
        };

        placemark.AddStyle(new Style
        {
            Line = new LineStyle
            {
                Color = alignmentDirection != SiteAlignmentDirection.NorthSouth
                        ? Color32.Parse(EastWestLineColor)
                        : Color32.Parse(NorthSouthLineColor),
                Width = 1
            }
        });

        return placemark;
    }
}