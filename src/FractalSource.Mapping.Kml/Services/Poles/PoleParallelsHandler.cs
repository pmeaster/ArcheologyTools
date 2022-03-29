using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleParallelsHandler : Service<LocationEntity>, IPoleParallelsHandler
{
    private readonly IPoleGridLineHandler _poleGridLineHandler;

    public PoleParallelsHandler(IPoleGridLineHandler poleGridLineHandler, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _poleGridLineHandler = poleGridLineHandler;
    }

    public KmlFeatureContainer HandlePoleLocationParallels(LocationEntity location)
    {
        return HandlePoleLocationParallelsAsync(location)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandlePoleLocationParallelsAsync(LocationEntity location)
    {
        var folder = new Folder
        {
            Name = "Pole Parallel Lines"
        };

        await HandlePoleParallelsAsync(location, folder);

        return folder.ToFeatureContainer();
    }

    private async Task HandlePoleParallelsAsync(LocationEntity location, Folder parentFolder)
    {
        const double sixDegrees = 667435D; 

        for (var i = 1; i < 15; i++)
        {
            var radius = sixDegrees * i;

            var gridLineName = $"{90 - (6 * i)}° North";

            parentFolder.AddFeature(
               await _poleGridLineHandler
                    .HandlePoleGridLineAsync(location, radius, gridLineName)
               );
        }

        for (var i = 1; i <= 15; i++)
        {
            var radius = sixDegrees * (i + 15);

            var gridLineName = $"-{6 * i}° South";

            parentFolder.AddFeature(
                await _poleGridLineHandler
                    .HandlePoleGridLineAsync(location, radius, gridLineName)
                );
        }
    }
}