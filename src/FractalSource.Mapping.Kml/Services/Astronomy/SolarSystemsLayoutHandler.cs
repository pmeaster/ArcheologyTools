using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal class SolarSystemsLayoutHandler : Service<LocationEntity, KmlFeatureContainer>, ISolarSystemsLayoutHandler
{
    private readonly ISolarSystemConfigurationProvider _solarSystemConfigurationProvider;
    private readonly ISolarSystemLayoutHandler _solarSystemLayoutHandler;

    public SolarSystemsLayoutHandler(ISolarSystemConfigurationProvider solarSystemConfigurationProvider,ISolarSystemLayoutHandler solarSystemLayoutHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _solarSystemConfigurationProvider = solarSystemConfigurationProvider;
        _solarSystemLayoutHandler = solarSystemLayoutHandler;
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
            Name = ISolarSystemsLayoutHandler.LayoutName,
            Description = new Description
            {
                Text = ISolarSystemsLayoutHandler.LayoutDescription
            }
        };

        var configurations 
            = await _solarSystemConfigurationProvider.GetRecordsAsync();

        foreach (var solarSystemConfiguration in configurations)
        {
            folder.AddFeature(
                await _solarSystemLayoutHandler.HandleLayoutAsync(location, solarSystemConfiguration, useNetworkLinks, useAntipode)
                );
        }

        return folder.ToFeatureContainer();
    }
}