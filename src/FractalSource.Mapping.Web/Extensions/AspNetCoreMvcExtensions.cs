// ReSharper disable CheckNamespace

using System.Net;
using FractalSource.Mapping;
using SharpKml.Dom;

namespace Microsoft.AspNetCore.Mvc;

public static class AspNetCoreMvcExtensions
{
    private const string KmlContentTypeName = "application/vnd.google-earth.kml+xml.kml";
    private const string KmzContentTypeName = "application/vnd.google-earth.kmz.kmz";

    public static ContentResult ToContentResult(this Feature feature)
    {
        return new ContentResult
        {
            Content = feature.ToXDocument().ToString(),
            ContentType = KmlContentTypeName,
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}