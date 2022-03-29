// ReSharper disable CheckNamespace

using FractalSource.Mapping.Data.Entities;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web;

public static class LocationExtensions
{
    public static NetworkLink GetNetworkLink(this LocationEntity location, string url)
    {
        return 
            location.GetNetworkLink(location.Name, location.Description, url);
    }

    public static NetworkLink GetNetworkLink(this LocationEntity location, Uri uri)
    {
        return
            location.GetNetworkLink(location.Name, location.Description, uri);
    }

    public static NetworkLink GetNetworkLink(this LocationEntity location, string linkName, 
        string linkDescription, string url)
    {
        var uri = new Uri(url, UriKind.RelativeOrAbsolute);

        return 
            location.GetNetworkLink(linkName, linkDescription, uri);
    }

    public static NetworkLink GetNetworkLink(this LocationEntity location, string linkName,
        string linkDescription, Uri uri)
    {
        return new NetworkLink
        {
            Id = location.InstanceId.ToString(),
            Name = linkName,
            Description = new Description
            {
                Text = linkDescription
            },
            Visibility = false,
            Link = new Link
            {
                Href = uri,
                RefreshMode = RefreshMode.OnChange,
                ViewRefreshMode = ViewRefreshMode.Never
            },
            RefreshVisibility = false
        };
    }
}