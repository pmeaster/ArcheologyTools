using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

internal class LantisZonesMeasurementSystemLayoutHandler : Service<LocationEntity, Feature>, ILantisZonesMeasurementSystemLayoutHandler
{
    private readonly ILantisZoneProvider _lantisZoneProvider;
    private readonly ILantisZoneLineHandler _lantisZoneLineHandler;
    private readonly ILantisMeasurementSystemNetworkLinkProvider _lantisMeasurementSystemNetworkLinkProvider;

    public LantisZonesMeasurementSystemLayoutHandler(ILantisZoneProvider lantisZoneProvider, ILantisZoneLineHandler lantisZoneLineHandler, 
        ILantisMeasurementSystemNetworkLinkProvider lantisMeasurementSystemNetworkLinkProvider,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _lantisZoneProvider = lantisZoneProvider;
        _lantisZoneLineHandler = lantisZoneLineHandler;
        _lantisMeasurementSystemNetworkLinkProvider = lantisMeasurementSystemNetworkLinkProvider;
    }

    public Feature HandleLayout(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useNetworkLinks = false, bool useAntipode = false)
    {
        return HandleLayoutAsync(location, measurementSystem, useNetworkLinks, useAntipode)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useNetworkLinks = false, bool useAntipode = false)
    {

        if (useNetworkLinks)
        {
            return _lantisMeasurementSystemNetworkLinkProvider
                .GetNetworkLink(location, measurementSystem, useAntipode);
        }

        var formatParameters = new List<object>
        {
            Environment.NewLine,
            measurementSystem.Name,
            measurementSystem.Description,
            measurementSystem.Abbreviation,
            measurementSystem.BaseRatio,
            measurementSystem.StadiaAbbreviation
        };

        var systemFolder = new Folder
        {
            Name = measurementSystem.Name,
            Description = new Description
            {
                Text = string.Format(
                    ILantisZonesMeasurementSystemLayoutHandler.FolderDescription,
                    formatParameters.ToArray())
            }
        };

        var zones
            = (await _lantisZoneProvider.GetRecordsAsync()).ToList();

        foreach (var zone in zones)
        {
            systemFolder.AddFeature(
                await _lantisZoneLineHandler.HandleLantisZoneLineAsync(location, measurementSystem, zone)
            );
        }

        return systemFolder;
    }
}