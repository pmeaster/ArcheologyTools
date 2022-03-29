using System;
using FractalSource.Mapping.Services.Geodesy;
using FractalSource.Services;
using Microsoft.Extensions.Logging;

namespace FractalSource.Mapping.Geodesy
{
    public class GeodeticCalculatorFactory : ServiceFactory, IGeodeticCalculatorFactory
    {
        public GeodeticCalculatorFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        public GeodeticCalculator CreateGeodeticCalculator()
        {
            return new GeodeticCalculator(Ellipsoid.Wgs84);
        }

        public GeodeticCalculator CreateGeodeticCalculator(Ellipsoid referenceGlobe)
        {
            return new GeodeticCalculator(referenceGlobe);
        }
    }
}