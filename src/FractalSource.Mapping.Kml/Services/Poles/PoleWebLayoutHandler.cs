using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleWebLayoutHandler : Service<LocationEntity, Feature>, IPoleWebLayoutHandler
{
    private readonly IPoleLayoutHandler _poleLayoutHandler;
    private readonly ILantisZonesWebLayoutHandler _lantisZonesWebLayoutHandler;
    private readonly ISolarSystemsWebLayoutHandler _solarSystemsWebLayoutHandler;

    public PoleWebLayoutHandler(IPoleLayoutHandler poleLayoutHandler, ILantisZonesWebLayoutHandler lantisZonesWebLayoutHandler, 
        ISolarSystemsWebLayoutHandler solarSystemsWebLayoutHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _poleLayoutHandler = poleLayoutHandler;
        _lantisZonesWebLayoutHandler = lantisZonesWebLayoutHandler;
        _solarSystemsWebLayoutHandler = solarSystemsWebLayoutHandler;
    }

    public Feature HandleLayout(LocationEntity location)
    {
        return HandleLayoutAsync(location)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location)
    {
        var container = await _poleLayoutHandler.HandleLayoutAsync(location);
        
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

        var layoutsFolder = folder.AddFolder("Layouts");
        //layoutsFolder.Visibility = false;

        layoutsFolder.AddFeature(
            await _lantisZonesWebLayoutHandler.HandleLayoutAsync(location)
        );

        layoutsFolder.AddFeature(
            await _solarSystemsWebLayoutHandler.HandleLayoutAsync(location)
        );

        var antipodesFolder = folder.AddFolder("Antipode Layouts");
        //antipodesFolder.Visibility = false;

        antipodesFolder.AddFeature(
            await _lantisZonesWebLayoutHandler.HandleLayoutAsync(location, true)
        );

        antipodesFolder.AddFeature(
            await _solarSystemsWebLayoutHandler.HandleLayoutAsync(location, true)
        );

        return folder;
    }
}