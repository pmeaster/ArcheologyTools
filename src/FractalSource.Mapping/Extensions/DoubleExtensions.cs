using System;

// ReSharper disable once CheckNamespace
namespace FractalSource.Mapping.Geodesy
{
    public static class DoubleExtensions
    {
        public static bool IsApproximatelyEqualTo(this double a, double b,
            double delta = Constants.DefaultPrecision)
        {
            if (double.IsNaN(a)) return double.IsNaN(b);
            if (double.IsInfinity(a)) return double.IsInfinity(b);
            if (a.Equals(b)) return true;
            var scale = 1.0;
            if (!(a.Equals(0.0) || b.Equals(0.0)))
                scale = Math.Max(Math.Abs(a), Math.Abs(b));
            return Math.Abs(a - b) <= scale * delta;
        }

        public static bool IsSmaller(this double value1, double value2, double delta = Constants.DefaultPrecision)
        {
            if (value1.IsApproximatelyEqualTo(value2, delta))
                return false;
            return value1 < value2;
        }

        public static bool IsZero(this double val)
        {
            return (Math.Sign(val) == 0);
        }

        public static bool IsNegative(this double val)
        {
            return (Math.Sign(val) == -1);
        }

        public static bool IsPositive(this double val)
        {
            return (Math.Sign(val) == 1);
        }
    }
}