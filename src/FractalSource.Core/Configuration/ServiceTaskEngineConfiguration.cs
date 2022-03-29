using Microsoft.Extensions.Configuration;

namespace FractalSource.Configuration
{
    public class ServiceTaskEngineConfiguration : IServiceTaskEngineConfiguration
    {
        public ServiceTaskEngineConfiguration(IConfiguration configuration)
        {
            Name = IServiceTaskEngineConfiguration.SectionName;
            ServiceName = configuration.GetValue(() => ServiceName);
        }

        public string Name { get; }

        public string ServiceName { get; }
    }
}