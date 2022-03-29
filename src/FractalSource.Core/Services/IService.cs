using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public interface IService<TItem1, TItem2, TItem3, TItem4> : IService, IServiceKeyContainer<TItem1, TItem2, TItem3, TItem4>
    {
    }

    public interface IService<TItem1, TItem2, TItem3> : IService, IServiceKeyContainer<TItem1, TItem2, TItem3>
    {
    }

    public interface IService<TItem1, TItem2> : IService, IServiceKeyContainer<TItem1, TItem2>
    {
    }

    public interface IService<TItem> : IService, IServiceKeyContainer<TItem>
    {
    }

    public interface IService : IServiceItem
    {
        ILogger Logger { get; }
    }
}