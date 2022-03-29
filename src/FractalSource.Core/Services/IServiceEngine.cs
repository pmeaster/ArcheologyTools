using System;
using Microsoft.Extensions.Hosting;

namespace FractalSource.Services
{
    public interface IServiceEngine : IService, IHostedService, IDisposable
    {

    }
}