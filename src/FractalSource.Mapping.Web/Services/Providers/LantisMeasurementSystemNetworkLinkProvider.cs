using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Web.Controllers;
using FractalSource.Services;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Services.Providers;

internal class LantisMeasurementSystemNetworkLinkProvider : Service<NetworkLink>, ILantisMeasurementSystemNetworkLinkProvider
{
    private readonly IUrlHelper _urlHelper;

    public LantisMeasurementSystemNetworkLinkProvider(IUrlHelper urlHelper,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _urlHelper = urlHelper;
    }


    public NetworkLink GetNetworkLink(LocationEntity location, MeasurementSystemEntity measurementSystem, bool useAntipode = false)
    {
        var linkUrlBase
            = _urlHelper.GetNetworkLinkUrl<LantisZonesController>(
                nameof(LantisZonesController.LantisMeasurementSystemLayout)
            );

        var uri = new Uri(
            $"{linkUrlBase}?locationId={location.ID}&locationType={location.LocationType}" +
            $"&measurementSystemId={measurementSystem.ID}&useAntipode={useAntipode}",
            UriKind.RelativeOrAbsolute);

        var formatParameters = new List<object>
        {
            Environment.NewLine,
            measurementSystem.Name,
            measurementSystem.Description,
            measurementSystem.Abbreviation,
            measurementSystem.BaseRatio,
            measurementSystem.StadiaAbbreviation
        };

        var description 
            = string.Format(
                ILantisZonesMeasurementSystemLayoutHandler.FolderDescription, 
                formatParameters.ToArray()
                );

        return 
            location.GetNetworkLink(measurementSystem.Name, description, uri);
    }
}