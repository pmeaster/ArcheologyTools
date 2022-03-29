using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FractalSource.Mapping.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FractalSourceHostBuilder
                .Build(args)
                .Run();
        }
    }
}
