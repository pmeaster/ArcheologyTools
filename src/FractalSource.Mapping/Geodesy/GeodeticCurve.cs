using System;
using System.Globalization;
using System.Text;

namespace FractalSource.Mapping.Geodesy
{
    public readonly struct GeodeticCurve : IEquatable<GeodeticCurve>
    {
        public double EllipsoidalDistance { get; }

        public GeodeticCalculator Calculator { get; }

        public Angle Azimuth { get; }

        internal GeodeticCurve(
            GeodeticCalculator geoCalculator,
            double ellipsoidalDistance,
            Angle azimuth)
        {
            Calculator = geoCalculator;
            EllipsoidalDistance = ellipsoidalDistance;
            Azimuth = azimuth;
        }

        public Angle ReverseAzimuth
        {
            get
            {
                return Azimuth.Degrees switch
                {
                    double.NaN => double.NaN,
                    < 180.0 => Azimuth + 180.0,
                    _ => Azimuth - 180.0
                };
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("s=");
            builder.Append(EllipsoidalDistance.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";a12=");
            builder.Append(Azimuth);
            builder.Append(";a21=");
            builder.Append(ReverseAzimuth);
            builder.Append(";");

            return builder.ToString();
        }

        public bool Equals(GeodeticCurve other)
        {
            return EllipsoidalDistance.IsApproximatelyEqualTo(other.EllipsoidalDistance) 
                   && Azimuth.Equals(other.Azimuth) 
                   && Calculator.Equals(other.Calculator);
        }
    }
}