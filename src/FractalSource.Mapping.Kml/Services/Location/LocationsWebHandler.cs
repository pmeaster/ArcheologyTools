using System;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public class LocationsWebHandler : Service<LocationEntity, Feature>, ILocationsWebHandler
{
    private readonly ILocationProvider _locationProvider;
    private readonly ILocationWebHandler _locationWebHandler;

    public LocationsWebHandler(ILocationProvider locationProvider, ILocationWebHandler locationWebHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationProvider = locationProvider;
        _locationWebHandler = locationWebHandler;
    }

    public Feature HandleLocations(LocationType locationType, bool useNetworkLinks = false)
    {
        return HandleLocationsAsync(locationType, useNetworkLinks)
            .Result;
    }

    public async Task<Feature> HandleLocationsAsync(LocationType locationType, bool useNetworkLinks = false)
    {
        //Do not send back a network link for the parent folder. 
        var locations
            = await _locationProvider.GetRecordsAsync(locationType);

        string folderName;
        string folderDescription;

        switch (locationType)
        {
            case LocationType.Lantis:
                folderName = LocationsWebLayoutFolder.LantisFolderName;
                folderDescription = LocationsWebLayoutFolder.LantisFolderDescription;
                break;

            case LocationType.Pole:
                folderName = LocationsWebLayoutFolder.PoleFolderName;
                folderDescription = LocationsWebLayoutFolder.PoleFolderDescription;
                break;

            case LocationType.Site:
                folderName = LocationsWebLayoutFolder.SiteFolderName;
                folderDescription = LocationsWebLayoutFolder.SiteFolderDescription;
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(locationType), 
                    locationType, 
                    null);
        }

        var folder = new Folder
        {
            Name = folderName,
            Description = new Description
            {
                Text = folderDescription
            }
        };

        foreach (var location in locations)
        {
            folder.AddFeature(
                await _locationWebHandler.HandleLocationAsync(location, useNetworkLinks)
            );
        }

        return folder;
    }
}