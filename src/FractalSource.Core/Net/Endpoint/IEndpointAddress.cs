using System;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public interface IEndpointAddress<TDescription> : IService<TDescription>
        where TDescription : class, IEndpointDescription
    {
        TDescription Description { get; }

        Uri GetAddressUri();
    }
}