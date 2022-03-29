using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal class SolarSystemOrbitLayoutHandler : Service<LocationEntity, Folder>, ISolarSystemOrbitLayoutHandler
{
    private readonly ISolarSystemOrbitBoundaryHandler _solarSystemOrbitBoundaryHandler;

    public SolarSystemOrbitLayoutHandler(ISolarSystemOrbitBoundaryHandler solarSystemOrbitBoundaryHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _solarSystemOrbitBoundaryHandler = solarSystemOrbitBoundaryHandler;
    }

    public Folder HandleLayout(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        return HandleLayoutAsync(locationEntity, solarSystemObjectRadius)
            .Result;
    }

    public async Task<Folder> HandleLayoutAsync(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        var objectFolder = new Folder
        {
            Name = solarSystemObjectRadius.Name
        };
 
        objectFolder.AddFeature(
            await _solarSystemOrbitBoundaryHandler
                .HandleOrbitBoundaryAsync(locationEntity, solarSystemObjectRadius, AxisRadiusType.Minimum)
            );

        objectFolder.AddFeature(
            await _solarSystemOrbitBoundaryHandler
                .HandleOrbitBoundaryAsync(locationEntity, solarSystemObjectRadius, AxisRadiusType.Average)
        );

        objectFolder.AddFeature(
            await _solarSystemOrbitBoundaryHandler
                .HandleOrbitBoundaryAsync(locationEntity, solarSystemObjectRadius, AxisRadiusType.Maximum)
        );

        objectFolder.AddFeature(
            await _solarSystemOrbitBoundaryHandler
                .HandleOrbitBoundaryAsync(locationEntity, solarSystemObjectRadius, AxisRadiusType.Normalized)
        );

        return objectFolder;
    }
}