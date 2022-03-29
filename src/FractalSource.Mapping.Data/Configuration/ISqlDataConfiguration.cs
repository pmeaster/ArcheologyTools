using FractalSource.Configuration;

namespace FractalSource.Mapping.Configuration;

public interface ISqlDataConfiguration : IHasConfigurationName
{
    public static string SectionName => nameof(ISqlDataConfiguration);

    string ConnectionString { get; }
}