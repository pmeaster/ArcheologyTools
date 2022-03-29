using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedType(Format.Native,
    Name = nameof(SqlGeodeticCurve))]
public readonly struct SqlGeodeticCurve : INullable
{
    private readonly double _distance;
    private readonly double _azimuth;

    public SqlGeodeticCurve(double azimuth, double distance)
    {
        _azimuth = azimuth;

        _distance = distance;
    }

    public override string ToString()
    {
        return IsNull
            ? string.Empty
            : $"{Azimuth}:{Distance}";
    }

    public bool IsNull => (!Distance.HasValue || !Azimuth.HasValue);

    public double? Distance
    {
        get { return _distance; }
    }

    public double? Azimuth
    {
        get { return _azimuth; }
    }
    public static SqlGeodeticCurve Null
    {
        get
        {
            var curve = new SqlGeodeticCurve();

            return curve;
        }
    }

    public static SqlGeodeticCurve Parse(SqlString sqlString)
    {
        if (sqlString.IsNull)
        {
            return Null;
        }

        var list = sqlString.Value.Split(':');

        if (list.Length <= 0)
        {
            return new SqlGeodeticCurve();
        }

        return
            new SqlGeodeticCurve(
                double.Parse(list[0]),
                double.Parse(list[1]));
    }
}