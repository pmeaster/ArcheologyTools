using System.Threading.Tasks;
using FractalSource.Services;
using SharpKml.Dom;

namespace FractalSource.Mapping.Services.Location;

public interface ISiteAlignmentsWebHandler : IService<Feature>
{
    Feature HandleSiteAlignments(bool useNetworkLinks = false);

    Task<Feature> HandleSiteAlignmentsAsync(bool useNetworkLinks = false);
}