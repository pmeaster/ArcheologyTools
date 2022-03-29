using System.Collections.Generic;

namespace FractalSource.Net.Endpoint
{
    public interface IHasEndpointParameters
    {
        IEnumerable<IEndpointParameter> GetParameters();
    }
}