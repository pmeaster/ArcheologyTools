using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

internal interface IPoleGridLineHandler : IService<LocationEntity, Placemark>
{
    Placemark HandlePoleGridLine(LocationEntity location, double radius, 
        string gridLineName = PoleKmlStyles.GridLineName, double lineWidth = PoleKmlStyles.GridLineWidth);

    Task<Placemark> HandlePoleGridLineAsync(LocationEntity location, double radius, 
        string gridLineName = PoleKmlStyles.GridLineName, double lineWidth = PoleKmlStyles.GridLineWidth);
}