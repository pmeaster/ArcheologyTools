using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FractalSource.Mapping.Keyhole;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Keyhole
{
    public class KmlOutputHandler : Service<IEnumerable<KmlDocument>>, IKmlOutputHandler
    {
        public KmlOutputHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public void HandleKmlOutput(IEnumerable<KmlDocument> documents, string directoryName)
        {
            HandleKmlOutputAsync(documents, directoryName).RunSynchronously();
        }

        public async Task HandleKmlOutputAsync(IEnumerable<KmlDocument> documents, string directoryName)
        {
            //TODO: Change/Add support to save as Kmz (compressed kml)
            var outputDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\Output\\{directoryName}";

            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            foreach (var document in documents)
            {
                document.XDocument.Save($"{outputDirectory}\\{document.Name}.kml");
            }

            await Task.CompletedTask;
        }
    }
}