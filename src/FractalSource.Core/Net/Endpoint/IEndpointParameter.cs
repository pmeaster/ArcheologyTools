using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointParameter<TValue> : IEndpointParameter, INamedValueItem<TValue>
    {
    }

    public interface IEndpointParameter : IServiceItem
    {
        string GetParameterString();
    }
}