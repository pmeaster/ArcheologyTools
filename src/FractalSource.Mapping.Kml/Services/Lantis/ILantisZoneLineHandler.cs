using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Lantis;

public interface ILantisZoneLineHandler : IService<LocationEntity, MeasurementSystemEntity, LantisZoneEntity, Folder>
{
    Folder HandleLantisZoneLine(LocationEntity location, MeasurementSystemEntity measurementSystemEntity, LantisZoneEntity lantisZoneEntity);

    Task<Folder> HandleLantisZoneLineAsync(LocationEntity location, MeasurementSystemEntity measurementSystemEntity, LantisZoneEntity lantisZoneEntity);
}