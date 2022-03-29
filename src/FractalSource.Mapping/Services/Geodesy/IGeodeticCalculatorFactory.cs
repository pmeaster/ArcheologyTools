using FractalSource.Mapping.Geodesy;
using FractalSource.Services;

namespace FractalSource.Mapping.Services.Geodesy
{
    public interface IGeodeticCalculatorFactory : IServiceFactory
    {
        GeodeticCalculator CreateGeodeticCalculator();

        GeodeticCalculator CreateGeodeticCalculator(Ellipsoid referenceGlobe);
    }
}