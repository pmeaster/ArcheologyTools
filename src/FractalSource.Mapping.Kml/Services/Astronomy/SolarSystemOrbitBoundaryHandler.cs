using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Geometry;
using FractalSource.Services;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;
// ReSharper disable UnusedMember.Local

namespace FractalSource.Mapping.Services.Astronomy;

internal class SolarSystemOrbitBoundaryHandler : Service<LocationEntity, Placemark>, ISolarSystemOrbitBoundaryHandler
{

    private const string ColorAlpha15 = "26";
    private const string ColorAlpha25 = "40";
    private const string ColorAlpha40 = "66";
    private const string ColorAlpha50 = "80";
    private const string ColorAlpha75 = "CC";
    private const int DefaultLineWidth = 4;
    private const double DefaultRadiusLimit = 1000000D;
    private const double DefaultForcedAltitude = 500000D;

    private readonly ILayoutPlacemarkHandler _layoutPlacemarkHandler;

    public SolarSystemOrbitBoundaryHandler(ILayoutPlacemarkHandler layoutPlacemarkHandler,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _layoutPlacemarkHandler = layoutPlacemarkHandler;
    }

    public Placemark HandleOrbitBoundary(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius,
        AxisRadiusType axisRadiusType)
    {
        return HandleOrbitBoundaryAsync(locationEntity, solarSystemObjectRadius, axisRadiusType)
            .Result;
    }

    public async Task<Placemark> HandleOrbitBoundaryAsync(LocationEntity locationEntity, SolarSystemObjectRadiusEntity solarSystemObjectRadius, AxisRadiusType axisRadiusType)
    {
        var coordinates = locationEntity.Coordinates.Clone();

        switch (axisRadiusType)
        {
            case AxisRadiusType.Maximum:
                return
                    await HandleMaxOrbitBoundaryAsync(coordinates, solarSystemObjectRadius);

            case AxisRadiusType.Minimum:
                return
                    await HandleMinOrbitBoundaryAsync(coordinates.Clone(), solarSystemObjectRadius);

            case AxisRadiusType.Average:
                return
                    await HandleAvgOrbitBoundaryAsync(coordinates.Clone(), solarSystemObjectRadius);

            case AxisRadiusType.Normalized:
                return
                    await HandleNormalizedOrbitBoundaryAsync(coordinates.Clone(), solarSystemObjectRadius);

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(axisRadiusType),
                    axisRadiusType,
                    null);
        }
    }

    private async Task<Placemark> HandleMinOrbitBoundaryAsync(GeoCoordinates coordinates, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        var placemarkName = $"{solarSystemObjectRadius.Name} Min Orbit";
        var placemarkDescription
            = $"Radius = {Math.Round(solarSystemObjectRadius.MinPRatioRadius / 1000D, 2)} " +
              $"{solarSystemObjectRadius.MeasurementSystem.StadiaAbbreviation} (stadia).";

        var altitudeMode = KmlAltitudeMode.ClampToGround;

        //HACK: Fixes 'disappearing' line issue in Google Earth
        var radiusInMeters = solarSystemObjectRadius.MinPRatioRadius * solarSystemObjectRadius.MeasurementSystem.BaseRatio;

        if (radiusInMeters > DefaultRadiusLimit)
        {
            altitudeMode = KmlAltitudeMode.Absolute;
            coordinates.Altitude = DefaultForcedAltitude;
        }

        return
            await HandleOrbitBoundaryGeometryAsync(
                coordinates,
                solarSystemObjectRadius,
                placemarkName,
                placemarkDescription,
                new List<double>
                {
                    solarSystemObjectRadius.MinPRatioRadius
                },
                ColorAlpha75,
                kmlAltitudeMode: altitudeMode);
    }

    private async Task<Placemark> HandleAvgOrbitBoundaryAsync(GeoCoordinates coordinates, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        var placemarkName = $"{solarSystemObjectRadius.Name} Avg Orbit";
        var placemarkDescription
            = $"Radius = {Math.Round(solarSystemObjectRadius.AvgPRatioRadius / 1000D, 2)} " +
              $"{solarSystemObjectRadius.MeasurementSystem.StadiaAbbreviation} (stadia).";

        var altitudeMode = KmlAltitudeMode.ClampToGround;

        //HACK: Fixes 'disappearing' line issue in Google Earth
        var radiusInMeters = solarSystemObjectRadius.AvgPRatioRadius * solarSystemObjectRadius.MeasurementSystem.BaseRatio;

        if (radiusInMeters > DefaultRadiusLimit)
        {
            altitudeMode = KmlAltitudeMode.Absolute;
            coordinates.Altitude = DefaultForcedAltitude;
        }

        return
            await HandleOrbitBoundaryGeometryAsync(
                coordinates,
                solarSystemObjectRadius,
                placemarkName,
                placemarkDescription,
                new List<double>
                {
                    solarSystemObjectRadius.AvgPRatioRadius
                },
                ColorAlpha75,
                kmlAltitudeMode: altitudeMode);
    }

    private async Task<Placemark> HandleMaxOrbitBoundaryAsync(GeoCoordinates coordinates, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        var placemarkName = $"{solarSystemObjectRadius.Name} Max Orbit";
        var placemarkDescription
            = $"Radius = {Math.Round(solarSystemObjectRadius.MaxPRatioRadius / 1000D, 2)} " +
              $"{solarSystemObjectRadius.MeasurementSystem.StadiaAbbreviation} (stadia).";

        var altitudeMode = KmlAltitudeMode.ClampToGround;

        //HACK: Fixes 'disappearing' line issue in Google Earth
        var radiusInMeters = solarSystemObjectRadius.MaxPRatioRadius * solarSystemObjectRadius.MeasurementSystem.BaseRatio;

        if (radiusInMeters > DefaultRadiusLimit)
        {
            altitudeMode = KmlAltitudeMode.Absolute;
            coordinates.Altitude = DefaultForcedAltitude;
        }

        return
            await HandleOrbitBoundaryGeometryAsync(
                coordinates,
                solarSystemObjectRadius,
                placemarkName,
                placemarkDescription,
                new List<double>
                {
                    solarSystemObjectRadius.MaxPRatioRadius
                },
                ColorAlpha75,
                kmlAltitudeMode: altitudeMode);
    }

    private async Task<Placemark> HandleNormalizedOrbitBoundaryAsync(GeoCoordinates coordinates, SolarSystemObjectRadiusEntity solarSystemObjectRadius)
    {
        var placemarkName = $"{solarSystemObjectRadius.Name} Avg Orbit";
        var placemarkDescription
            = $"Radius = {Math.Round(solarSystemObjectRadius.AvgPRatioRadius / 1000D, 2)} " +
              $"{solarSystemObjectRadius.MeasurementSystem.StadiaAbbreviation} (stadia).";

        var eccentricityRatio
            = solarSystemObjectRadius.MinPRatioRadius /
              (solarSystemObjectRadius.MaxPRatioRadius != 0
                  ? solarSystemObjectRadius.MaxPRatioRadius : solarSystemObjectRadius.MinPRatioRadius);

        coordinates.Altitude = solarSystemObjectRadius.OrbitAltitude;

        return
            await HandleOrbitBoundaryGeometryAsync(
                coordinates,
                solarSystemObjectRadius,
                placemarkName,
                placemarkDescription,
                new List<double>
                {
                    solarSystemObjectRadius.AvgPRatioRadius
                },
                ColorAlpha75,
                eccentricityRatio: eccentricityRatio,
                degreesToRotate: solarSystemObjectRadius.OrbitRotation,
                kmlAltitudeMode: (solarSystemObjectRadius.OrbitAltitude != 0 ? KmlAltitudeMode.Absolute : KmlAltitudeMode.ClampToGround),
                inclination: solarSystemObjectRadius.OrbitInclination);
    }

    private async Task<Placemark> HandleOrbitBoundaryGeometryAsync(GeoCoordinates coordinates, SolarSystemObjectRadiusEntity solarSystemObjectRadius,
        string placemarkName, string placemarkDescription, IEnumerable<double> radii, string objectAlphaColor,
        double lineWidth = DefaultLineWidth, double eccentricityRatio = Constants.ZeroEccentricity, double degreesToRotate = 0,
        KmlAltitudeMode kmlAltitudeMode = KmlAltitudeMode.ClampToGround, bool extrude = false, double inclination = 0)
    {
        var placemark = new KmlPlacemark
        {
            Name = placemarkName,
            Description = placemarkDescription,
            PlacemarkType = KmlPlacemarkType.Line,
            Coordinates = coordinates,
            Style =
            {
                LabelSize = default,
                Icon =
                {
                    Size = default,
                    Url = default
                }
            }
        };

        var geometry = new KmlGeometry
        {
            Name = placemark.Name,
            Description = placemark.Description,
            Radii = radii,
            MeasurementSystemRatio = solarSystemObjectRadius.MeasurementSystem.BaseRatio,
            Ellipse =
            {
                Eccentricity = eccentricityRatio,
                DegreesToRotate = degreesToRotate,
                Inclination = inclination
            },
            LineStyle =
            {
                Color = $"{objectAlphaColor}{solarSystemObjectRadius.ObjectBaseColor}",
                Width = lineWidth
            },
            PolygonStyle =
            {
                Color = $"{objectAlphaColor}{solarSystemObjectRadius.ObjectBaseColor}",
                Fill = default,
                Outline = default
            },
            Altitude =
            {
                AltitudeMode = kmlAltitudeMode,
                Extrude = extrude
            }
        };

        return
            await _layoutPlacemarkHandler.HandlePlaceMarkAsync(placemark, geometry);
    }
}