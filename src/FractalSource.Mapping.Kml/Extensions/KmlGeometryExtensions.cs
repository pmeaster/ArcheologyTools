using System;
using System.Collections.Generic;
using System.Linq;
using FractalSource.Mapping.Keyhole;
using SharpKml.Base;
using SharpKml.Dom;

namespace FractalSource.Mapping;

public static class KmlGeometryExtensions
{
    public static Geometry ToPolygon(this IEnumerable<IEnumerable<GeoCoordinates>> coordinatesLists,
        KmlAltitudeMode altitudeMode = KmlAltitudeMode.ClampToGround, bool extrude = false)
    {
        return coordinatesLists.SelectMany(c => c)
            .To3DPolygon(Enumerable.Empty<GeoCoordinates>(),
                altitudeMode,
                extrude);
    }

    public static Geometry To3DPolygon(this IEnumerable<GeoCoordinates> outerCoordinates,
        IEnumerable<GeoCoordinates> innerCoordinates, KmlAltitudeMode altitudeMode = KmlAltitudeMode.ClampToGround,
        bool extrude = false)
    {
        var outerList = outerCoordinates.ToList();
        var innerList = innerCoordinates.ToList();

        var polygon = new Polygon
        {
            AltitudeMode = altitudeMode.ToAltitudeMode(),
            Extrude = extrude,
            Tessellate = altitudeMode == KmlAltitudeMode.ClampToGround,
            OuterBoundary = new OuterBoundary
            {
                LinearRing = new LinearRing
                {
                    Coordinates = outerList.ToCoordinateCollection(),
                    Tessellate = altitudeMode == KmlAltitudeMode.ClampToGround
                }
            }
        };

        if (innerList.Any())
        {
            polygon.AddInnerBoundary(new InnerBoundary
            {
                LinearRing = new LinearRing
                {
                    Coordinates = innerList.ToCoordinateCollection(),
                    Tessellate = altitudeMode == KmlAltitudeMode.ClampToGround
                }
            });
        }

        return polygon;
    }

    public static Geometry To3DPolygon(this IEnumerable<IEnumerable<GeoCoordinates>> coordinates,
        KmlAltitudeMode altitudeMode = KmlAltitudeMode.ClampToGround, bool extrude = false, bool useMultiGeometry = false)
    {
        var coordinatesLists = coordinates.ToList();

        if (!useMultiGeometry)
        {
            return coordinatesLists.ToPolygon(altitudeMode, extrude);
        }

        var coordinatesPairs = new List<Tuple<IEnumerable<GeoCoordinates>,
            IEnumerable<GeoCoordinates>>>();

        for (var i = 0; i < coordinatesLists.Count; i += 2)
        {
            var outerCoordinates = coordinatesLists[i];
            var innerCoordinates = i + 1 < coordinatesLists.Count
                ? coordinatesLists[i + 1]
                : Enumerable.Empty<GeoCoordinates>();

            coordinatesPairs.Add(
                new Tuple<IEnumerable<GeoCoordinates>,
                    IEnumerable<GeoCoordinates>>(
                    outerCoordinates, innerCoordinates));
        }

        var geometry = new MultipleGeometry();

        foreach (var coordinatesPair in coordinatesPairs)
        {
            geometry.AddGeometry(
                coordinatesPair.Item1.To3DPolygon(coordinatesPair.Item2, altitudeMode, extrude)
            );
        }

        return geometry;
    }

    public static Geometry ToSphere(this IEnumerable<IEnumerable<GeoCoordinates>> coordinates,
        KmlAltitudeMode altitudeMode = KmlAltitudeMode.RelativeToGround, bool extrude = false)
    {
        var geometry = new MultipleGeometry();

        var coordinatesLists = coordinates.ToList();

        var coordinatesPairs = new List<Tuple<IEnumerable<GeoCoordinates>, IEnumerable<GeoCoordinates>>>();

        var previousCoordinatesList = default(IEnumerable<GeoCoordinates>);

        for (var i = 0; i < coordinatesLists.Count; i += 2)
        {
            var outerCoordinates = coordinatesLists[i].ToList();
            var innerCoordinates = i + 1 < coordinatesLists.Count
                ? coordinatesLists[i + 1]
                : Enumerable.Empty<GeoCoordinates>();

            coordinatesPairs.Add(
                new Tuple<IEnumerable<GeoCoordinates>,
                    IEnumerable<GeoCoordinates>>(
                    outerCoordinates, innerCoordinates));

            if (previousCoordinatesList != null)
            {
                coordinatesPairs.Add(
                    new Tuple<IEnumerable<GeoCoordinates>,
                        IEnumerable<GeoCoordinates>>(
                        previousCoordinatesList, outerCoordinates));
            }

            previousCoordinatesList = outerCoordinates;
        }

        foreach (var coordinatesPair in coordinatesPairs)
        {
            geometry.AddGeometry(
                coordinatesPair.Item1.To3DPolygon(coordinatesPair.Item2,
                    altitudeMode,
                    extrude)
            );
        }

        return geometry;
    }

    public static Geometry ToLine(this IEnumerable<GeoCoordinates> coordinates,
        KmlAltitudeMode altitudeMode = KmlAltitudeMode.ClampToGround, bool extrude = false,
        int? drawOrder = null)
    {
        var coordinatesList = coordinates.ToList();

        var coordinateCollection = coordinatesList.ToCoordinateCollection();

        var lineString = new LineString
        {
            AltitudeMode = altitudeMode.ToAltitudeMode(),
            Extrude = extrude,
            Tessellate = altitudeMode == KmlAltitudeMode.ClampToGround,
            Coordinates = coordinateCollection,
            GXDrawOrder = drawOrder
        };

        return lineString;
    }

    public static Geometry ToPoint(this GeoCoordinates coordinates)
    {

        var point = new Point
        {
            Coordinate = new Vector
            {
                Altitude = coordinates.Altitude,
                Latitude = coordinates.Latitude,
                Longitude = coordinates.Longitude
            }
        };

        return point;
    }

    public static CoordinateCollection ToCoordinateCollection(this IEnumerable<GeoCoordinates> coordinates)
    {
        var coordinateCollection = new CoordinateCollection();

        var vectors = coordinates.Select(c => new Vector(c.Latitude, c.Longitude, c.Altitude));

        foreach (var vector in vectors)
        {
            coordinateCollection.Add(vector);
        }

        return coordinateCollection;
    }

    public static AltitudeMode ToAltitudeMode(this KmlAltitudeMode altitudeMode)
    {
        return altitudeMode switch
        {
            KmlAltitudeMode.ClampToGround => AltitudeMode.ClampToGround,
            KmlAltitudeMode.RelativeToGround => AltitudeMode.RelativeToGround,
            KmlAltitudeMode.Absolute => AltitudeMode.Absolute,
            _ => throw new ArgumentOutOfRangeException(nameof(altitudeMode), altitudeMode, null)
        };
    }
}