using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

internal class LantisZonesWebLayoutHandler : Service<LocationEntity, Feature>, ILantisZonesWebLayoutHandler
{
    private readonly ILantisZonesLayoutHandler _lantisZonesLayoutHandler;

    public LantisZonesWebLayoutHandler(ILantisZonesLayoutHandler lantisZonesLayoutHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _lantisZonesLayoutHandler = lantisZonesLayoutHandler;
    }

    public Feature HandleLayout(LocationEntity location, bool useAntipode = false)
    {
        return HandleLayoutAsync(location, useAntipode)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location, bool useAntipode = false)
    {
        const bool useNetworkLinks = true;

        var container 
            = await _lantisZonesLayoutHandler.HandleLayoutAsync(location, useNetworkLinks, useAntipode);

        var feature = container.ToFeature();

        var folder = (feature as Container) ??
                     new Folder
                     {
                         Name = feature.Name,
                         Description = feature.Description
                     };

        if (feature is not Container)
        {
            folder.AddFeature(feature);
        }

        return folder;
    }
}