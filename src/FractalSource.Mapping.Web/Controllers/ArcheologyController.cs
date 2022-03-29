using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Location;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ArcheologyController : MappingController<ArcheologyController>
{
    private const bool UseNetworkLinks = true;

    private readonly ILocationsWebHandler _locationsWebHandler;
    private readonly ISiteAlignmentsWebHandler _siteAlignmentsWebHandler;

    public ArcheologyController(ILocationsWebHandler locationsWebHandler, ISiteAlignmentsWebHandler siteAlignmentsWebHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationsWebHandler = locationsWebHandler;
        _siteAlignmentsWebHandler = siteAlignmentsWebHandler;
    }

    [HttpGet(nameof(NetworkLinkRoot))]
    public async Task<ContentResult> NetworkLinkRoot()
    {
        var document = new Document
        {
            Name = "Archaeological Renaissance",
            Description = new Description
            {
                Text = "The true history of humanity."
            }
        };

        document.AddFeature(
            await _locationsWebHandler.HandleLocationsAsync(LocationType.Pole, UseNetworkLinks)
           );

        document.AddFeature(
            await _locationsWebHandler.HandleLocationsAsync(LocationType.Site, UseNetworkLinks)
           );

        document.AddFeature(
            await _locationsWebHandler.HandleLocationsAsync(LocationType.Lantis, UseNetworkLinks)
            );

        return document.ToContentResult();
    }

    [HttpGet(nameof(Poles))]
    public async Task<ContentResult> Poles()
    {

        return 
            (await _locationsWebHandler.HandleLocationsAsync(LocationType.Pole, UseNetworkLinks))
            .ToContentResult();
    }

    [HttpGet(nameof(Sites))]
    public async Task<ContentResult> Sites()
    {

        return
            (await _locationsWebHandler.HandleLocationsAsync(LocationType.Site, UseNetworkLinks))
            .ToContentResult();
    }

    [HttpGet(nameof(LantisLocations))]
    public async Task<ContentResult> LantisLocations()
    {

        return
            (await _locationsWebHandler.HandleLocationsAsync(LocationType.Lantis, UseNetworkLinks))
            .ToContentResult();
    }

    [HttpGet(nameof(SiteAlignments))]
    public async Task<ContentResult> SiteAlignments()
    {

        return
            (await _siteAlignmentsWebHandler.HandleSiteAlignmentsAsync(UseNetworkLinks))
            .ToContentResult();
    }
}