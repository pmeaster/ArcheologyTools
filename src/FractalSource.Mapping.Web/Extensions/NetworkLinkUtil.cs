using Microsoft.AspNetCore.Mvc;
// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Web;

public static class NetworkLinkUtil
{
    public static string GetControllerName<TController>()
        where TController : ControllerBase
    {
        return
            typeof(TController).Name
                .Replace("Controller",
                    string.Empty,
                    StringComparison.InvariantCultureIgnoreCase);
    }

    public static Uri GetNetworkLinkHref<TController>(this IUrlHelper urlHelper, string actionName, 
        object? values = null)
        where TController : ControllerBase
    {

        return 
            new Uri(
                urlHelper.GetNetworkLinkUrl<TController>(actionName, values), 
                UriKind.RelativeOrAbsolute);
    }

    public static string GetNetworkLinkUrl<TController>(this IUrlHelper urlHelper, string actionName, 
        object? values = null)
        where TController : ControllerBase
    {

        return
            urlHelper.ActionLink(
                actionName, 
                GetControllerName<TController>(), 
                values) 
            ?? string.Empty;

    }
}