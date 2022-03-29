using System;
using FractalSource.Mapping.Geodesy;
using FractalSource.Services;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FractalSource.Mapping
{
    public class GeoCoordinates : ServiceItem, IEquatable<GeoCoordinates>
    {
        public GeoCoordinates()
        {
            Canonicalize(0, 0);

            Altitude = 0;
        }

        public GeoCoordinates(double latitude, double longitude, double altitude = 0)
        {
            Canonicalize(latitude, longitude);

            Altitude = altitude;
        }

        public Angle Latitude { get; private set; }

        public Angle Longitude { get; private set; }

        public double Altitude { get; set; }

        public GeoCoordinates GetAntipode()
        {
            var latitude = Latitude.Degrees * -1;
            var longitude = (180 - Math.Abs(Longitude.Degrees)) * (Longitude.Degrees < 0 ? 1 : -1);

            return new GeoCoordinates(latitude, longitude, Altitude);
        }

        public GeoCoordinates Clone()
        {
            return new GeoCoordinates()
            {
                Altitude = Altitude,
                Latitude = Latitude,
                Longitude = Longitude
            };
        }

        private void Canonicalize(double latitude, double longitude)
        {
            latitude = (latitude + 180) % 360;
            if (latitude.IsNegative()) latitude += 360;
            latitude -= 180;

            switch (latitude)
            {
                case > 90:
                    latitude = 180 - latitude;
                    longitude += 180;

                    break;

                case < -90:
                    latitude = -180 - latitude;
                    longitude += 180;

                    break;
            }

            longitude = ((longitude + 180) % 360);

            if (longitude < 0)
            {
                longitude += 360;
            }

            longitude -= 180;

            Latitude = new Angle(latitude);

            Longitude = new Angle(longitude);
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is not GeoCoordinates coordinate
                // ReSharper disable once BaseObjectEqualsIsObjectEquals
                ? base.Equals(obj)
                : Equals(coordinate);
        }

        public override string ToString()
        {
            return $"{Latitude}, {Longitude}, {Altitude} m";
        }

        public bool Equals(GeoCoordinates other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return Latitude.Equals(other.Latitude)
                   && Longitude.Equals(other.Longitude);
        }

        public static bool operator ==(GeoCoordinates left, GeoCoordinates right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(GeoCoordinates left, GeoCoordinates right)
        {
            return !(left == right);
        }
    }
}