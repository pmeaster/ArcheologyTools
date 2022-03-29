namespace FractalSource.Configuration
{
    public interface IServiceTaskEngineConfiguration : IHasConfigurationName
    {
        public static string SectionName => nameof(IServiceTaskEngineConfiguration);

        string ServiceName { get; }
    }
}