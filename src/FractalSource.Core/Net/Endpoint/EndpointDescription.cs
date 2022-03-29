using System.Collections.Generic;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointDescription : ServiceItem, IEndpointDescription
    {
        public IEnumerable<IEndpointParameter> GetParameters()
        {
            return OnGetParameters();
        }

        protected abstract IEnumerable<IEndpointParameter> OnGetParameters();
    }
}