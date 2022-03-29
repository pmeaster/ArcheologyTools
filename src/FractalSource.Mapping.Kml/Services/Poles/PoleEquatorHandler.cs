using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal class PoleEquatorHandler : Service<LocationEntity>, IPoleEquatorHandler
{
    private readonly IPoleGridLineHandler _poleGridLineHandler;

    public PoleEquatorHandler(IPoleGridLineHandler poleGridLineHandler, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _poleGridLineHandler = poleGridLineHandler;
    }

    public void HandlePoleEquator(LocationEntity location, Folder parentFolder)
    {
        HandlePoleEquatorAsync(location, parentFolder)
            .Wait();
    }

    public async Task HandlePoleEquatorAsync(LocationEntity location, Folder parentFolder)
    {
        var equatorPlacemark =
            await _poleGridLineHandler.HandlePoleGridLineAsync(location, 
                PoleParallels.Equator, nameof(PoleParallels.Equator), PoleKmlStyles.EquatorWidth);

        parentFolder.AddFeature(equatorPlacemark);
    }

    public KmlFeatureContainer HandlePoleLocationEquator(LocationEntity location)
    {
        return HandlePoleLocationEquatorAsync(location)
            .Result;
    }

    public async Task<KmlFeatureContainer> HandlePoleLocationEquatorAsync(LocationEntity location)
    {
        var equatorPlacemark =
            await _poleGridLineHandler.HandlePoleGridLineAsync(location,
                PoleParallels.Equator, nameof(PoleParallels.Equator), PoleKmlStyles.EquatorWidth);

        return equatorPlacemark.ToFeatureContainer();
    }
}