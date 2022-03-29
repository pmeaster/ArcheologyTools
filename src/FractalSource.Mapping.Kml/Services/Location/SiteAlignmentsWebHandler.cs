using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Sites;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

internal class SiteAlignmentsWebHandler : Service<Feature>, ISiteAlignmentsWebHandler
{
    private readonly ILocationProvider _locationProvider;
    private readonly ISiteAlignmentsHandler _siteAlignmentsHandler;

    public SiteAlignmentsWebHandler(ILocationProvider locationProvider, ISiteAlignmentsHandler siteAlignmentsHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationProvider = locationProvider;
        _siteAlignmentsHandler = siteAlignmentsHandler;
    }

    public Feature HandleSiteAlignments(bool useNetworkLinks = false)
    {
        return HandleSiteAlignmentsAsync(useNetworkLinks)
            .Result;
    }

    public async Task<Feature> HandleSiteAlignmentsAsync(bool useNetworkLinks = false)
    {
        var locations
            = await _locationProvider.GetRecordsAsync(LocationType.Site);

        var sites = locations.Cast<SiteLocationEntity>();

        var folder = new Folder
        {
            Id = InstanceId.ToString(),
            Name = LocationsWebLayoutFolder.SiteAlignmentsFolderName,
            Description = new Description
            {
                Text = LocationsWebLayoutFolder.SiteAlignmentsFolderDescription
            }
        };

        var northSouthFolder = folder.AddFolder("North/South Alignments");

        var eastWestFolder = folder.AddFolder("East/West Alignments");

        foreach (var site in sites)
        {
            if (site.North.HasValue)
            {
                northSouthFolder.AddFeature(
                    (await _siteAlignmentsHandler.HandleSiteAlignmentsAsync(site, SiteAlignmentDirection.NorthSouth))
                    .ToFeature()
                    );
            }

            if (site.East.HasValue)
            {
                eastWestFolder.AddFeature(
                    (await _siteAlignmentsHandler.HandleSiteAlignmentsAsync(site, SiteAlignmentDirection.EastWest))
                    .ToFeature()
                );
            }
        }

        return folder;
    }
}