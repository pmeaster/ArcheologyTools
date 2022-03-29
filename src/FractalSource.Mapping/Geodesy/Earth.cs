using System;

namespace FractalSource.Mapping.Geodesy
{
    public static class Earth
    {
        /*

        Equatorial radius (m)               6378137     
        Polar radius (m)                    6356752         
        Volumetric mean radius (m)          6371000
        Haversine Formula radius (m)        6376500

         */
        public const double PolarRadius = 6367445.0;

        public const double EquatorialRadius = 6378137.0;

        public const double OrbitEccentricity = 0.0167;

        public const double Circumference = 2 * (Math.PI * PolarRadius);

        public const double MetersPerDegree = (1 / ((2 * Math.PI / 360) * PolarRadius));

        public static readonly GeoCoordinates Equator = new(0, 0);

        public static readonly GeoCoordinates NorthPole = new(90, 0);

        public static readonly GeoCoordinates SouthPole = NorthPole.GetAntipode();
    }
}