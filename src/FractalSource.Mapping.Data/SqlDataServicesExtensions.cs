using FractalSource.Data;
using FractalSource.Mapping.Configuration;
using FractalSource.Mapping.Data.Context;
using FractalSource.Mapping.Data.Repository;
using FractalSource.Mapping.Data.Services;
using FractalSource.Mapping.Services.Astronomy;
using FractalSource.Mapping.Services.Lantis;
using FractalSource.Mapping.Services.Location;
using FractalSource.Mapping.Services.MeasurementSystem;
using FractalSource.Mapping.Services.Poles;
using FractalSource.Mapping.Services.Sites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace FractalSource.Mapping.Services;

public static class SqlDataServicesExtensions
{
    public static IServiceCollection AddFractalSourceMappingSqlDataServices(this IServiceCollection services)
    {
        return services
            .AddSqlDbContext()
            .AddTransient<ILocationProvider, LocationProvider>()
            .AddTransient<IPoleLocationProvider, PoleLocationProvider>()
            .AddTransient<ILantisLocationProvider, LantisLocationProvider>()
            .AddTransient<ILantisZoneProvider, LantisZoneProvider>()
            .AddTransient<ISiteLocationProvider, SiteLocationProvider>()
            .AddTransient<ISolarSystemObjectRadiusProvider, SolarSystemObjectRadiusProvider>()
            .AddTransient<ISolarSystemConfigurationProvider, SolarSystemConfigurationProvider>()
            .AddTransient<IMeasurementSystemProvider, MeasurementSystemProvider>()
            .AddTransient<IMeasurementSystemExpandedProvider, MeasurementSystemExpandedProvider>()
            .AddTransient(typeof(IRepository<>), typeof(SqlRepository<>));

    }

    internal static IServiceCollection AddSqlDbContext(this IServiceCollection services)
    {
        services.AddTransient<ArcheologyContext>();
        services.AddTransient<IArcheologyContextFactory, ArcheologyContextFactory>();

        return services
            .AddDbContext<DbContext, ArcheologyContext>((serviceProvider, optionsBuilder) =>
                {
                    var configuration =
                        ActivatorUtilities
                            .GetServiceOrCreateInstance<ISqlDataConfiguration>(serviceProvider);

                    optionsBuilder.UseSqlServer(configuration.ConnectionString, (options) =>
                    {
                        options.EnableRetryOnFailure();
                    });
                },
                ServiceLifetime.Transient,
                ServiceLifetime.Transient);
    }
}