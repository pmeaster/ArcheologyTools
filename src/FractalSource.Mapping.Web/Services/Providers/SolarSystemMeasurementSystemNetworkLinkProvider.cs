using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Web.Controllers;
using FractalSource.Services;
using Microsoft.AspNetCore.Mvc;
using SharpKml.Dom;

namespace FractalSource.Mapping.Web.Services.Providers;

internal class SolarSystemMeasurementSystemNetworkLinkProvider : Service<NetworkLink>, ISolarSystemMeasurementSystemNetworkLinkProvider
{
    private readonly IUrlHelper _urlHelper;

    public SolarSystemMeasurementSystemNetworkLinkProvider(IUrlHelper urlHelper, 
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        _urlHelper = urlHelper;
    }

    public NetworkLink GetNetworkLink(LocationEntity location, SolarSystemConfigurationEntity solarSystemConfiguration, MeasurementSystemEntity measurementSystem, bool useAntipode = false)
    {
        var linkUrlBase
            = _urlHelper.GetNetworkLinkUrl<SolarSystemsController>(
                nameof(SolarSystemsController.SolarSystemMeasurementSystemLayout)
            );

        var uri = new Uri(
            $"{linkUrlBase}?locationId={location.ID}&locationType={location.LocationType}&solarSystemConfigurationId={solarSystemConfiguration.ID}&measurementSystemId={measurementSystem.ID}&useAntipode={useAntipode}",
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
                ISolarSystemLayoutMeasurementSystemHandler.FolderDescription,
                formatParameters.ToArray()
            );

        return
            location.GetNetworkLink(measurementSystem.Name, description, uri);
    }
}