using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.MeasurementSystem;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

internal class LantisZonesLayoutHandler : Service<LocationEntity, KmlFeatureContainer>, ILantisZonesLayoutHandler
{
    private readonly IMeasurementSystemProvider _measurementSystemProvider;
    private readonly ILantisZonesMeasurementSystemLayoutHandler _lantisZonesMeasurementSystemLayoutHandler;

    public LantisZonesLayoutHandler(IMeasurementSystemProvider measurementSystemProvider, 
        ILantisZonesMeasurementSystemLayoutHandler lantisZonesMeasurementSystemLayoutHandler, 
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _measurementSystemProvider = measurementSystemProvider;
        _lantisZonesMeasurementSystemLayoutHandler = lantisZonesMeasurementSystemLayoutHandler;
    }

    public KmlFeatureContainer HandleLayout(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false)
    {
        return HandleLayoutAsync(location, useNetworkLinks, useAntipode)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandleLayoutAsync(LocationEntity location, bool useNetworkLinks = false, bool useAntipode = false)
    {
        var folder = new Folder
        {
            Name = ILantisZonesLayoutHandler.LayoutName,
            Description = new Description
            {
                Text = ILantisZonesLayoutHandler.LayoutDescription
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
            var feature
                = await _lantisZonesMeasurementSystemLayoutHandler
                    .HandleLayoutAsync(location, measurementSystem, useNetworkLinks, useAntipode);

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

        return folder.ToFeatureContainer();
    }
}