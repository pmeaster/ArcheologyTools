using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.MeasurementSystem;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal class SolarSystemLayoutHandler : Service<LocationEntity, Feature>, ISolarSystemLayoutHandler
{
    private readonly IMeasurementSystemProvider _measurementSystemProvider;
    private readonly ISolarSystemLayoutMeasurementSystemHandler _solarSystemLayoutMeasurementSystemHandler;
    private readonly ISolarSystemMeasurementSystemNetworkLinkProvider _solarSystemMeasurementSystemNetworkLinkProvider;

    public SolarSystemLayoutHandler(IMeasurementSystemProvider measurementSystemProvider, ISolarSystemLayoutMeasurementSystemHandler solarSystemLayoutMeasurementSystemHandler,
        ISolarSystemMeasurementSystemNetworkLinkProvider solarSystemMeasurementSystemNetworkLinkProvider,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _measurementSystemProvider = measurementSystemProvider;
        _solarSystemLayoutMeasurementSystemHandler = solarSystemLayoutMeasurementSystemHandler;
        _solarSystemMeasurementSystemNetworkLinkProvider = solarSystemMeasurementSystemNetworkLinkProvider;
    }

    public Feature HandleLayout(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, bool useNetworkLinks = false, bool useAntipode = false)
    {
        return HandleLayoutAsync(location, solarSystemConfiguration, useNetworkLinks, useAntipode)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, bool useNetworkLinks = false, bool useAntipode = false)
    {
        var folder = new Folder
        {
            Name = solarSystemConfiguration.Name,
            Description = new Description
            {
                Text = solarSystemConfiguration.Description
            }
        };

        var measurementSystems
            = await _measurementSystemProvider.GetRecordsAsync();

        const bool visibility = false;

        var antediluvianFolder
            = folder.AddFolder($"{nameof(MeasurementSystemCategory.Antediluvian)} Systems", visibility);

        var standardFolder
            = folder.AddFolder($"{nameof(MeasurementSystemCategory.Standard)} Systems", visibility);

        var numericalFolder
            = folder.AddFolder($"{nameof(MeasurementSystemCategory.Numerical)} Systems", visibility);

        var projectionFolder
            = folder.AddFolder($"{nameof(MeasurementSystemCategory.Projection)} Systems", visibility);

        var anunnakiFolder
            = folder.AddFolder($"{nameof(MeasurementSystemCategory.Anunnaki)} Systems", visibility);

        foreach (var measurementSystem in measurementSystems)
        {

            var feature = !useNetworkLinks
                ? await _solarSystemLayoutMeasurementSystemHandler
                    .HandleLayoutAsync(location, solarSystemConfiguration, measurementSystem)
                : _solarSystemMeasurementSystemNetworkLinkProvider
                    .GetNetworkLink(location, solarSystemConfiguration, measurementSystem, useAntipode);

            var systemFolder = antediluvianFolder;

            switch (measurementSystem.Category)
            {
                case MeasurementSystemCategory.Standard:
                    systemFolder = standardFolder;
                    break;
                case MeasurementSystemCategory.Projection:
                    systemFolder = projectionFolder;
                    break;
                case MeasurementSystemCategory.Numerical:
                    systemFolder = numericalFolder;
                    break;
                case MeasurementSystemCategory.Anunnaki:
                    systemFolder = anunnakiFolder;
                    break;
            }

            systemFolder.AddFeature(feature);

        }

        return folder;
    }
}