using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Web.Controllers;
using FractalSource.Services;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Services.Providers;

internal class SolarSystemsNetworkLinkProvider : Service<NetworkLink>, ISolarSystemsNetworkLinkProvider
{
    private readonly IUrlHelper _urlHelper;

    public SolarSystemsNetworkLinkProvider(IUrlHelper urlHelper, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _urlHelper = urlHelper;
    }

    public NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false)
    {
        var linkUrlBase
            = _urlHelper.GetNetworkLinkUrl<SolarSystemsController>(
                nameof(SolarSystemsController.SolarSystemsLayout)
            );

        var uri = new Uri(
            $"{linkUrlBase}?locationId={location.ID}&locationType={location.LocationType}&useAntipode={useAntipode}",
            UriKind.RelativeOrAbsolute);

        return
            location.GetNetworkLink(
                ISolarSystemsLayoutHandler.LayoutName,
                ISolarSystemsLayoutHandler.LayoutDescription,
                uri);
    }
}