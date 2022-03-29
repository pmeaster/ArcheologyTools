using System.Collections.Generic;
using System.Threading.Tasks;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Mapping.Keyhole;
using FractalSource.Mapping.Services.Location;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Services.Poles
{
    internal class PolesTaskHandler : Service<IEnumerable<LocationEntity>, KmlDocument>, IPolesTaskHandler
    {
        private readonly ILocationHandler _locationHandler;

        public PolesTaskHandler(ILocationHandler locationHandler, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _locationHandler = locationHandler;
        }

        public KmlDocument HandlePoleLocations(IEnumerable<LocationEntity> locations)
        {
            return HandlePoleLocationsAsync(locations)
                .Result;
        }

        public async Task<KmlDocument> HandlePoleLocationsAsync(IEnumerable<LocationEntity> locations)
        {
            var kmlDocument = new KmlDocument
            {
                Name = "Axis Symmetry Pivot (Pole Shift)",
                Description = "Locations, Equators, Parallels, and Meridians of the Various External Axis of Earth.",
            };

            var document = kmlDocument.ToDocument();

            foreach (var location in locations)
            {
                var folder
                    = await _locationHandler.HandleLocationAsync(location);

                document.AddFeature(folder.ToFeature());
            }

            kmlDocument.XDocument = document.ToXDocument();

            return kmlDocument;
        }
    }
}