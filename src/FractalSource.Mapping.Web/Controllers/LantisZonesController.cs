using FractalSource.Mapping.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.MeasurementSystem;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LantisZonesController : MappingController<LantisZonesController>
{
    private readonly ILocationProvider _locationProvider;
    private readonly ILantisZonesWebLayoutHandler _lantisZonesWebLayoutHandler;
    private readonly ILantisZonesMeasurementSystemLayoutHandler _lantisZonesMeasurementSystemLayoutHandler;
    private readonly IMeasurementSystemProvider _measurementSystemProvider;

    public LantisZonesController(ILocationProvider locationProvider, ILantisZonesWebLayoutHandler lantisZonesWebLayoutHandler, 
        ILantisZonesMeasurementSystemLayoutHandler lantisZonesMeasurementSystemLayoutHandler, IMeasurementSystemProvider measurementSystemProvider, 
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationProvider = locationProvider;
        _lantisZonesWebLayoutHandler = lantisZonesWebLayoutHandler;
        _lantisZonesMeasurementSystemLayoutHandler = lantisZonesMeasurementSystemLayoutHandler;
        _measurementSystemProvider = measurementSystemProvider;
    }

    [HttpGet(nameof(LantisZonesLayout))]
    public async Task<ContentResult> LantisZonesLayout(int locationId, LocationType locationType, bool useAntipode)
    {
        return
            (await GetLantisZonesFeature(locationId, locationType, useAntipode))
            .ToContentResult();
    }

    private async Task<Feature> GetLantisZonesFeature(int locationId, LocationType locationType, bool useAntipode)
    {
        var location = await _locationProvider.GetLocationAsync(locationId, locationType);

        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        return
            await _lantisZonesWebLayoutHandler.HandleLayoutAsync(location, useAntipode);
    }

    [HttpGet(nameof(LantisMeasurementSystemLayout))]
    public async Task<ContentResult> LantisMeasurementSystemLayout(int locationId, LocationType locationType, int measurementSystemId, bool useAntipode)
    {
        return
            (await GetLantisZonesMeasurementSystemFeature(locationId, locationType, measurementSystemId, useAntipode))
            .ToContentResult();
    }

    private async Task<Feature> GetLantisZonesMeasurementSystemFeature(int locationId, LocationType locationType, int measurementSystemId, bool useAntipode)
    {
        const bool useNetworkLinks = false;

        var location = await _locationProvider.GetLocationAsync(locationId, locationType);

        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        var measurementSystem
            = await _measurementSystemProvider.GetMeasurementSystemAsync(measurementSystemId);

        return
            await _lantisZonesMeasurementSystemLayoutHandler.HandleLayoutAsync(location, measurementSystem, useNetworkLinks, useAntipode);
    }
}