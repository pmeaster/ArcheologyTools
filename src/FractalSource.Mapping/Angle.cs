using System;
using FractalSource.Mapping.Geodesy;

namespace FractalSource.Mapping
{
    public readonly struct Angle : IComparable<Angle>, IEquatable<Angle>
    {
        public Angle(double degrees)
        {
            Degrees = degrees;
        }

        public double Degrees { get; }

        public double Radians => Degrees * Constants.PiOver180;

        public bool IsNegative => Dms.IsNegative;

        public int Minutes => Dms.Minutes;

        public double Seconds => Dms.Seconds;

        public int WholeDegrees => Dms.Degrees;

        private DmsDegrees Dms => DmsDegrees.FromDecimalDegrees(Degrees);

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Constants.PiOver180;
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians / Constants.PiOver180;
        }

        public int CompareTo(Angle other)
        {
            if (Degrees.IsApproximatelyEqualTo(other.Degrees))
            {
                return 0;
            }

            if (Degrees.IsSmaller(other.Degrees))
            {
                return -1;
            }

            return 1;
        }

        public override int GetHashCode()
        {
            return Degrees.GetHashCode();
        }

        public bool Equals(Angle other)
        {
            return Degrees.IsApproximatelyEqualTo(other.Degrees);
        }

        public override bool Equals(object obj)
        {
            return obj is Angle angle && Equals(angle);
        }

        public string ToDmsString()
        {
            return Dms.ToString();
        }

        public override string ToString()
        {
            return $"{Degrees:##.0000000}°";
        }

        public static Angle operator +(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degrees + rhs.Degrees);
        }

        public static Angle operator -(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degrees - rhs.Degrees);
        }

        public static Angle operator *(double lhs, Angle rhs)
        {
            return new Angle(lhs * rhs.Degrees);
        }

        public static Angle operator *(Angle lhs, double rhs)
        {
            return new Angle(lhs.Degrees * rhs);
        }

        public static bool operator >(Angle lhs, Angle rhs)
        {
            return rhs.Degrees.IsSmaller(lhs.Degrees);
        }

        public static bool operator >=(Angle lhs, Angle rhs)
        {
            return rhs.Degrees.IsApproximatelyEqualTo(lhs.Degrees) ||
                   rhs.Degrees.IsSmaller(lhs.Degrees);
        }

        public static bool operator <(Angle lhs, Angle rhs)
        {
            return lhs.Degrees.IsSmaller(rhs.Degrees);
        }

        public static bool operator <=(Angle lhs, Angle rhs)
        {
            return lhs.Degrees.IsApproximatelyEqualTo(rhs.Degrees) ||
                   lhs.Degrees.IsSmaller(rhs.Degrees);
        }

        public static bool operator ==(Angle lhs, Angle rhs)
        {
            return
                lhs.Degrees.IsApproximatelyEqualTo(rhs.Degrees);
        }

        public static bool operator !=(Angle lhs, Angle rhs)
        {
            return
                !lhs.Degrees.IsApproximatelyEqualTo(rhs.Degrees);
        }

        public static Angle operator -(Angle unitary)
        {
            return new Angle(-unitary.Degrees);
        }

        public static implicit operator Angle(double degrees)
        {
            return new Angle(degrees);
        }

        public static implicit operator double(Angle degrees)
        {
            return degrees.Degrees;
        }

        private readonly struct DmsDegrees
        {
            private DmsDegrees(bool isNegative, int degrees, int minutes, double seconds)
            {
                IsNegative = isNegative;
                Degrees = degrees;
                Minutes = minutes;
                Seconds = seconds;
            }

            public bool IsNegative { get; }

            public int Degrees { get; }

            public int Minutes { get; }

            public double Seconds { get; }

            public static DmsDegrees FromDecimalDegrees(double degrees)
            {
                degrees = Math.Round(degrees, 7);

                var isNegative = degrees < 0;
                //switch the value to positive
                degrees = Math.Abs(degrees);

                var intDegrees = (int)Math.Floor(degrees);
                var delta = degrees - intDegrees;

                //gets minutes and seconds
                var seconds = 3600.0 * delta;
                var minutes = (int)Math.Floor(seconds / 60.0);

                var decimalSeconds = Math.Round(seconds - (minutes * 60), 8);

                return new DmsDegrees(isNegative, intDegrees, minutes, decimalSeconds);
            }

            public override string ToString()
            {
                return $"{Degrees}° {Minutes:00}' {Seconds:00.0000}\"";
            }
        }
    }
}