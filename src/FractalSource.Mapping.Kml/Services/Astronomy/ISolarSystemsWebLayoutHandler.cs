using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Astronomy;

public interface ISolarSystemsWebLayoutHandler : IService<LocationEntity, Feature>
{
    Feature HandleLayout(LocationEntity location, bool useAntipode = false);

    Task<Feature> HandleLayoutAsync(LocationEntity location, bool useAntipode = false);
}
