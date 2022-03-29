using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Sites;

public interface ISiteAlignmentsHandler : IService<SiteLocationEntity, KmlFeatureContainer>
{
    KmlFeatureContainer HandleSiteAlignments(SiteLocationEntity location, SiteAlignmentDirection alignmentDirection);

    Task<KmlFeatureContainer> HandleSiteAlignmentsAsync(SiteLocationEntity location, SiteAlignmentDirection alignmentDirection);
}