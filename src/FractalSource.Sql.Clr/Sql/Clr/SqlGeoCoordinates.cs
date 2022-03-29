using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedType(Format.Native,
    Name = nameof(SqlGeoCoordinates))]
public readonly struct SqlGeoCoordinates : INullable
{
    private readonly double _latitude;
    private readonly double _longitude;
    private readonly long _order;

    public SqlGeoCoordinates(double latitude, double longitude, long order = 0)
    {
        _latitude = latitude;
        _longitude = longitude;
        _order = order;
    }

    public override string ToString()
    {
        return IsNull
            ? string.Empty
            : $"{Latitude}:{Longitude}";
    }

    public bool IsNull => (!Longitude.HasValue || !Latitude.HasValue);

    internal long Order { get { return _order; } }

    public double? Latitude
    {
        get { return IsEmpty ? null : _latitude; }
    }

    public double? Longitude
    {
        get { return IsEmpty ? null : _longitude; }
    }

    private bool IsEmpty
    {
        get { return _latitude == 0 && _longitude == 0; }
    }

    public static SqlGeoCoordinates Null
    {
        get
        {
            var curve = new SqlGeoCoordinates();

            return curve;
        }
    }

    public static SqlGeoCoordinates Parse(SqlString sqlString)
    {
        if (sqlString.IsNull)
        {
            return Null;
        }

        var list = sqlString.Value.Split(':');

        if (list.Length <= 0)
        {
            return new SqlGeoCoordinates();
        }

        return
            new SqlGeoCoordinates(
                double.Parse(list[0]),
                double.Parse(list[1]));
    }
}