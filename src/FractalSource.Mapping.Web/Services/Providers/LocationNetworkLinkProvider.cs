using FractalSource.Mapping.Data;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Web.Controllers;
using FractalSource.Services;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Services.Providers;

internal class LocationNetworkLinkProvider : Service<LocationEntity, NetworkLink>, ILocationNetworkLinkProvider
{
    private readonly IUrlHelper _urlHelper;

    public LocationNetworkLinkProvider(IUrlHelper urlHelper, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _urlHelper = urlHelper;
    }

    public NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false)
    {
        return OnGetNetworkLink(location, useAntipode);
    }

    private NetworkLink OnGetNetworkLink(LocationEntity location, bool useAntipode = false)
    {
        if (useAntipode)
        {
            location = location.ConvertToAntipode();
        }

        var linkUrlBase
            = _urlHelper.GetNetworkLinkUrl<LocationController>(
                nameof(LocationController.LocationLayout)
            );

        var linkUrl = $"{linkUrlBase}?locationId={location.ID}&locationType={location.LocationType}" +
                      $"&useAntipode={useAntipode}";

        return
            location.GetNetworkLink(linkUrl);
    }

}