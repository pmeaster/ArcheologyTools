using FractalSource.Configuration;
using FractalSource.Data;
using FractalSource.Net.Endpoint;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace FractalSource.Services;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddFractalSourceCommonServices(this IServiceCollection services)
    {
        return services
            /* Core Services */
            //TODO: Create configuration to turn this on and off
            //.AddHostedService<ServiceTaskEngine>()
            .AddTransient<IRepositoryFactory, RepositoryFactory>()
            .AddTransient<IServiceTaskEngineConfiguration, ServiceTaskEngineConfiguration>()
            .AddTransient<IEndpointAddressFactory, EndpointAddressFactory>()
            .AddTransient<IEndpointDescriptionFactory, EndpointDescriptionFactory>()
            .AddTransient<IEndpointRecordProviderFactory, EndpointRecordProviderFactory>()
            .AddTransient<IEndpointServiceFactory, EndpointServiceFactory>()
            .AddTransient<IEndpointStreamHandlerFactory, EndpointStreamHandlerFactory>()
            .AddTransient<IRecordConverterFactory, RecordConverterFactory>()
            .AddTransient<IRecordHandlerFactory, RecordHandlerFactory>()
            .AddTransient(typeof(IRecordHandler<,>), typeof(RecordHandler<,>))
            .AddTransient(typeof(IRecordConverter<,>), typeof(RecordConverter<,>));
    }

}