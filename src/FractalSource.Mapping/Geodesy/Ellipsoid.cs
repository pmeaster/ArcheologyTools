using System;

namespace FractalSource.Mapping.Geodesy
{
    public readonly struct Ellipsoid : IEquatable<Ellipsoid>
    {
        public Ellipsoid(double semiMajor, double flattening)
        {
            SemiMajorAxis = semiMajor;
            Flattening = flattening;
        }

        public double SemiMajorAxis { get; }

        public double Flattening { get; }

        public double SemiMinorAxis => Ratio * SemiMajorAxis;

        // ReSharper disable once UnusedMember.Global
        public double InverseFlattening => 1.0 / Flattening;

        public double Ratio => 1.0 - Flattening;

        public double Circumference => 2 * Math.PI * SemiMajorAxis;

        public double Eccentricity => Math.Sqrt(1.0 - Math.Pow(Ratio, 2));

        public bool Equals(Ellipsoid other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Ellipsoid ellipsoid && Equals(ellipsoid);
        }

        public override int GetHashCode()
        {
            double[] xy = { SemiMajorAxis, SemiMajorAxis };
            return xy.GetHashCode();
        }

        public static bool operator ==(Ellipsoid lhs, Ellipsoid rhs)
        {
            return lhs.SemiMajorAxis.IsApproximatelyEqualTo(rhs.SemiMajorAxis) &&
                   lhs.Flattening.IsApproximatelyEqualTo(rhs.Flattening);
        }

        public static bool operator !=(Ellipsoid lhs, Ellipsoid rhs)
        {
            return !(lhs == rhs);
        }

        public static readonly Ellipsoid Wgs84 = new(6378137.0, 1.0 / 298.257223563);

        public static readonly Ellipsoid Grs80 = new(6378137.0, 1.0 / 298.257222101);

        public static readonly Ellipsoid Grs67 = new(6378160.0, 1.0 / 298.25);

        public static readonly Ellipsoid Ans = new(6378160.0, 1.0 / 298.25);

        public static readonly Ellipsoid Wgs72 = new(6378135.0, 1.0 / 298.26);

        public static readonly Ellipsoid Clarke1858 = new(6378293.645, 1.0 / 294.26);

        public static readonly Ellipsoid Clarke1880 = new(6378249.145, 1.0 / 293.465);

        public static readonly Ellipsoid Sphere = new(6371000, 0.0);

        public static readonly Ellipsoid Antediluvian = new(9549296.65, 0.00183471098165646694238764630712);

        //0.00183471098165646694238764630712

    }
}