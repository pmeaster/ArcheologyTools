using System;
using System.Globalization;
using System.Text;

namespace FractalSource.Mapping.Geodesy
{
    public readonly struct GeodeticMeasurement : IEquatable<GeodeticMeasurement>
    {
        public GeodeticCalculator Calculator => AverageCurve.Calculator;

        public double EllipsoidalDistance => AverageCurve.EllipsoidalDistance;

        public Angle Azimuth => AverageCurve.Azimuth;

        public Angle ReverseAzimuth => AverageCurve.ReverseAzimuth;

        public GeodeticCurve AverageCurve { get; }

        public double ElevationChange { get; }
        
        public double PointToPointDistance { get; }

        internal GeodeticMeasurement(
            GeodeticCurve averageCurve,
            double elevationChange)
        {
            var ellDist = averageCurve.EllipsoidalDistance;

            AverageCurve = averageCurve;
            ElevationChange = elevationChange;
            PointToPointDistance = Math.Sqrt(ellDist * ellDist + ElevationChange * ElevationChange);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(AverageCurve);
            builder.Append(";elev12=");
            builder.Append(ElevationChange.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";p2p=");
            builder.Append(PointToPointDistance.ToString(NumberFormatInfo.InvariantInfo));

            return builder.ToString();
        }

        public bool Equals(GeodeticMeasurement other)
        {
            return ElevationChange.IsApproximatelyEqualTo(other.ElevationChange) &&
                   AverageCurve.Equals(other.AverageCurve);
        }
    }
}