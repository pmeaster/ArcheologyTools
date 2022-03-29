using FractalSource.Mapping.Data.Context;
using FractalSource.Services;

namespace FractalSource.Mapping.Data.Services;

internal interface IArcheologyContextFactory : IServiceFactory
{
    ArcheologyContext CreateContext();
}