using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public class EndpointParameterName : ServiceItem, IEndpointParameterName
    {
        public string Value { get; set; }
    }
}
