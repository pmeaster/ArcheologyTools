using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Poles;

public interface IPoleGridLayoutHandler : IService<LocationEntity, Feature>
{
    Feature HandleLayout(LocationEntity location);

    Task<Feature> HandleLayoutAsync(LocationEntity location);
}