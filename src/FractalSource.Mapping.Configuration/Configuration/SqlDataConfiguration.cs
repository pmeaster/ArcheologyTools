using FractalSource.Configuration;
using Microsoft.Extensions.Configuration;

namespace FractalSource.Mapping.Configuration;

internal class SqlDataConfiguration : ISqlDataConfiguration
{
    public SqlDataConfiguration(IConfiguration configuration)
    {
        Name = ISqlDataConfiguration.SectionName;

        ConnectionString = configuration.GetValue(() => ConnectionString) ?? string.Empty;
    }

    public string Name { get; }

    public string ConnectionString { get; }
}