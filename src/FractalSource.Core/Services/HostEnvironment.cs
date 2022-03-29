using Microsoft.Extensions.Hosting;

namespace FractalSource.Services
{
    public static class HostEnvironment
    {
#pragma warning disable 8632
        public static IHost? CurrentHost { get; set; }
#pragma warning restore 8632

    }
}
