using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleGridLayoutHandler : Service<LocationEntity, Feature>, IPoleGridLayoutHandler
{
    private readonly IPoleEquatorHandler _poleEquatorHandler;
    private readonly IPoleParallelsHandler _poleParallelsHandler;
    private readonly IPoleMeridiansHandler _poleMeridiansHandler;

    public PoleGridLayoutHandler(IPoleEquatorHandler poleEquatorHandler,
        IPoleParallelsHandler poleParallelsHandler, 
        IPoleMeridiansHandler poleMeridiansHandler,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _poleEquatorHandler = poleEquatorHandler;
        _poleParallelsHandler = poleParallelsHandler;
        _poleMeridiansHandler = poleMeridiansHandler;
    }

    public Feature HandleLayout(LocationEntity location)
    {
        return HandleLayoutAsync(location)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity location)
    {
        var gridFolder = new Folder
        {
            Name = "Pole Grid",
            Description = new Description
            {
                Text = "Grid layout containing meridians, parallels, and equator lines."
            }
        };

        var equatorFolder = gridFolder.AddFolder("Equator");

        equatorFolder.AddFeature(
            (await _poleEquatorHandler.HandlePoleLocationEquatorAsync(location))
            .ToFeature()
        );

        var parallelsFolder = gridFolder.AddFolder("Parallels");

        parallelsFolder.AddFeature(
            (await _poleParallelsHandler.HandlePoleLocationParallelsAsync(location))
            .ToFeature()
        );

        var meridiansFolder = gridFolder.AddFolder("Meridians");

        meridiansFolder.AddFeature(
            (await _poleMeridiansHandler.HandlePoleLocationMeridiansAsync(location))
            .ToFeature()
        );

        return gridFolder;
    }
}