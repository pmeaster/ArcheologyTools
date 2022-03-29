using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Keyhole
{
    public interface IKmlOutputHandler : IService<IEnumerable<KmlDocument>>
    {
        void HandleKmlOutput(IEnumerable<KmlDocument> documents, string directoryName);

        Task HandleKmlOutputAsync(IEnumerable<KmlDocument> documents, string directoryName);
    }
}