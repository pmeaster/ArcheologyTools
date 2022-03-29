using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

internal class SolarSystemLayoutMeasurementSystemHandler : Service<LocationEntity, Feature>, ISolarSystemLayoutMeasurementSystemHandler
{
    private readonly ISolarSystemObjectRadiusProvider _systemObjectRadiusProvider;
    private readonly ISolarSystemOrbitBoundaryHandler _solarSystemOrbitBoundaryHandler;

    public SolarSystemLayoutMeasurementSystemHandler(ISolarSystemObjectRadiusProvider systemObjectRadiusProvider,
        ISolarSystemOrbitBoundaryHandler solarSystemOrbitBoundaryHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _systemObjectRadiusProvider = systemObjectRadiusProvider;
        _solarSystemOrbitBoundaryHandler = solarSystemOrbitBoundaryHandler;
    }


    public Feature HandleLayout(LocationEntity locationEntity, SolarSystemConfigurationEntity solarSystemConfiguration,
        MeasurementSystemEntity measurementSystem)
    {
        return HandleLayoutAsync(locationEntity, solarSystemConfiguration, measurementSystem)
            .Result;
    }

    public async Task<Feature> HandleLayoutAsync(LocationEntity locationEntity, SolarSystemConfigurationEntity solarSystemConfiguration,
        MeasurementSystemEntity measurementSystem)
    {
        var formatParameters = new List<object>
        {
            Environment.NewLine,
            measurementSystem.Name,
            measurementSystem.Description,
            measurementSystem.Abbreviation,
            measurementSystem.BaseRatio,
            measurementSystem.StadiaAbbreviation
        };

        var measurementSystemFolder = new Folder
        {
            Name = measurementSystem.Name,
            Description = new Description
            {
                Text = string.Format(
                    ISolarSystemLayoutMeasurementSystemHandler.FolderDescription,
                    formatParameters.ToArray())
            }
        };

        var solarObjectsOrbits
            = (await _systemObjectRadiusProvider.GetRecordsAsync(solarSystemConfiguration.ID, AxisType.Orbital))
            .ToList();

        var minOrbitsFolder
            = measurementSystemFolder.AddFolder("Minimum Orbits");

        var avgOrbitsFolder
            = measurementSystemFolder.AddFolder("Average Orbits");

        var maxOrbitsFolder
            = measurementSystemFolder.AddFolder("Maximum Orbits");

        var normalizedOrbitsFolder
            = measurementSystemFolder.AddFolder("Normalized Orbits");

        var normalized3DOrbitsFolder
            = measurementSystemFolder.AddFolder("Normalized 3D Orbits");

        foreach (var solarObjectOrbit in solarObjectsOrbits)
        {
            solarObjectOrbit.MeasurementSystem = measurementSystem;
            solarObjectOrbit.Configuration = solarSystemConfiguration;

            minOrbitsFolder.AddFeature(
                await _solarSystemOrbitBoundaryHandler
                    .HandleOrbitBoundaryAsync(locationEntity, solarObjectOrbit, AxisRadiusType.Minimum)
            );

            avgOrbitsFolder.AddFeature(
                await _solarSystemOrbitBoundaryHandler
                    .HandleOrbitBoundaryAsync(locationEntity, solarObjectOrbit, AxisRadiusType.Average)
            );

            maxOrbitsFolder.AddFeature(
                await _solarSystemOrbitBoundaryHandler
                    .HandleOrbitBoundaryAsync(locationEntity, solarObjectOrbit, AxisRadiusType.Maximum)
            );

            normalized3DOrbitsFolder.AddFeature(
                await _solarSystemOrbitBoundaryHandler
                    .HandleOrbitBoundaryAsync(locationEntity, solarObjectOrbit, AxisRadiusType.Normalized)
            );

            solarObjectOrbit.OrbitInclination = 0;
            solarObjectOrbit.OrbitAltitude = 0;

            normalizedOrbitsFolder.AddFeature(
                await _solarSystemOrbitBoundaryHandler
                    .HandleOrbitBoundaryAsync(locationEntity, solarObjectOrbit, AxisRadiusType.Normalized)
            );
        }

        //maxOrbitsFolder.MarkVisibilityRecursive(false);
        minOrbitsFolder.MarkVisibilityRecursive(false);
        avgOrbitsFolder.MarkVisibilityRecursive(false);
        normalized3DOrbitsFolder.MarkVisibilityRecursive(false);
        normalizedOrbitsFolder.MarkVisibilityRecursive(false);

        return measurementSystemFolder;
    }
}