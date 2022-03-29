using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public interface ILocationWebLayoutHandler : IService<LocationEntity, Feature>
{
    Feature HandleLayout(LocationEntity location, bool includeAntipode = true);

    Task<Feature> HandleLayoutAsync(LocationEntity location, bool includeAntipode = true);
}