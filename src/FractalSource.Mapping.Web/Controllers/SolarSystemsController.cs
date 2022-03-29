using FractalSource.Mapping.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.MeasurementSystem;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarSystemsController : MappingController<LantisZonesController>
{
    private readonly ILocationProvider _locationProvider;
    private readonly ISolarSystemConfigurationProvider _solarSystemConfigurationProvider;
    private readonly IMeasurementSystemProvider _measurementSystemProvider;
    private readonly ISolarSystemsWebLayoutHandler _solarSystemsWebLayoutHandler;
    private readonly ISolarSystemLayoutMeasurementSystemHandler _solarSystemLayoutMeasurementSystemHandler;

    public SolarSystemsController(ILocationProvider locationProvider, ISolarSystemConfigurationProvider solarSystemConfigurationProvider, 
        IMeasurementSystemProvider measurementSystemProvider, ISolarSystemsWebLayoutHandler solarSystemsWebLayoutHandler, 
        ISolarSystemLayoutMeasurementSystemHandler solarSystemLayoutMeasurementSystemHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _locationProvider = locationProvider;
        _solarSystemConfigurationProvider = solarSystemConfigurationProvider;
        _measurementSystemProvider = measurementSystemProvider;
        _solarSystemsWebLayoutHandler = solarSystemsWebLayoutHandler;
        _solarSystemLayoutMeasurementSystemHandler = solarSystemLayoutMeasurementSystemHandler;
    }

    [HttpGet(nameof(SolarSystemsLayout))]
    public async Task<ContentResult> SolarSystemsLayout(int locationId, LocationType locationType, bool useAntipode)
    {
        return
            (await GetSolarSystemsFeature(locationId, locationType, useAntipode))
            .ToContentResult();
    }

    private async Task<Feature> GetSolarSystemsFeature(int locationId, LocationType locationType, bool useAntipode)
    {
        var location = await _locationProvider.GetLocationAsync(locationId, locationType);

        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        return
            await _solarSystemsWebLayoutHandler.HandleLayoutAsync(location, useAntipode);
    }

    [HttpGet(nameof(SolarSystemMeasurementSystemLayout))]
    public async Task<ContentResult> SolarSystemMeasurementSystemLayout(int locationId, LocationType locationType, int solarSystemConfigurationId, 
        int measurementSystemId, bool useAntipode)
    {
        return
            (await GetSolarSystemMeasurementSystemFeature(locationId, locationType, 
                solarSystemConfigurationId, measurementSystemId, useAntipode))
            .ToContentResult();
    }

    private async Task<Feature> GetSolarSystemMeasurementSystemFeature(int locationId, LocationType locationType, int solarSystemConfigurationId, 
        int measurementSystemId, bool useAntipode)
    {
        var location = await _locationProvider.GetLocationAsync(locationId, locationType);

        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        var solarSystemConfiguration
            = await _solarSystemConfigurationProvider.GetSolarSystemConfigurationAsync(solarSystemConfigurationId);

        var measurementSystem
            = await _measurementSystemProvider.GetMeasurementSystemAsync(measurementSystemId);

        return
            await _solarSystemLayoutMeasurementSystemHandler.HandleLayoutAsync(location, solarSystemConfiguration, measurementSystem);
    }
}