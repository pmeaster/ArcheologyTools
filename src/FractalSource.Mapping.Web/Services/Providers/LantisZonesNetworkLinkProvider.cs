using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Web.Controllers;
using FractalSource.Services;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Services.Providers;

internal class LantisZonesNetworkLinkProvider : Service<NetworkLink>, ILantisZonesNetworkLinkProvider
{
    private readonly IUrlHelper _urlHelper;

    public LantisZonesNetworkLinkProvider(IUrlHelper urlHelper, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _urlHelper = urlHelper;
    }

    public NetworkLink GetNetworkLink(LocationEntity location, bool useAntipode = false)
    {
        var linkUrlBase
            = _urlHelper.GetNetworkLinkUrl<LantisZonesController>(
                nameof(LantisZonesController.LantisZonesLayout)
            );

        var uri = new Uri(
            $"{linkUrlBase}?locationId={location.ID}&locationType={location.LocationType}&useAntipode={useAntipode}", 
            UriKind.RelativeOrAbsolute);

        return
            location.GetNetworkLink(
                ILantisZonesLayoutHandler.LayoutName, 
                ILantisZonesLayoutHandler.LayoutDescription, 
                uri);
    }
}