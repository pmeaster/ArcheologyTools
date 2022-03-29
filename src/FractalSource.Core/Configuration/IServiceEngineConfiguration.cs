namespace FractalSource.Configuration
{
    public interface IServiceEngineConfiguration :  IHasConfigurationName
    {
        public static string SectionName => nameof(IServiceEngineConfiguration);

        int EndpointEnumerationDelay { get; }
    }
}