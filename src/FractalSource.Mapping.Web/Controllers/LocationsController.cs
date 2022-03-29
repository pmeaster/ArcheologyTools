using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Location;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : MappingController<LocationsController>
{
    private readonly ILocationsWebHandler _locationsWebHandler;

    public LocationsController(ILocationsWebHandler locationsWebHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationsWebHandler = locationsWebHandler;
    }

    [HttpGet(Name = nameof(GetLocations))]
    public async Task<ContentResult> GetLocations(LocationType locationType)
    {
        return
            (await GetSitesFeature(locationType))
            .ToContentResult();
    }

    private async Task<Feature> GetSitesFeature(LocationType locationType)
    {
        const bool useNetworkLinks = true;

        return
            await _locationsWebHandler.HandleLocationsAsync(locationType, useNetworkLinks);
    }
}