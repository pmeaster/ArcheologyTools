using FractalSource.Mapping.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Location;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : MappingController<LocationController>
{
    private readonly ILocationProvider _locationProvider;
    private readonly ILocationWebHandler _locationWebHandler;

    public LocationController(ILocationProvider locationProvider, ILocationWebHandler locationWebHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationProvider = locationProvider;
        _locationWebHandler = locationWebHandler;
    }

    [HttpGet(nameof(LocationLayout))]
    public async Task<ContentResult> LocationLayout(int locationId, LocationType locationType, bool useAntipode)
    {
        return
            (await GetLocationLayoutFeature(locationId, locationType, useAntipode))
            .ToContentResult();
    }

    private async Task<Feature> GetLocationLayoutFeature(int locationId, LocationType locationType, bool useAntipode)
    {
        var location = await _locationProvider.GetLocationAsync(locationId, locationType);

        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        return
            await _locationWebHandler.HandleLocationAsync(location, false, !useAntipode);
    }
}