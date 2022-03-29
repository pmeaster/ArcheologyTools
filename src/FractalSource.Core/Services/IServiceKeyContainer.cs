namespace FractalSource.Services
{
    public interface IServiceKeyContainer<TItem1, TItem2, TItem3, TItem4>
    {
        ServiceKeys<TItem1, TItem2, TItem3, TItem4> ServiceKeys { get; }
    }

    public interface IServiceKeyContainer<TItem1, TItem2, TItem3>
    {
        ServiceKeys<TItem1, TItem2, TItem3> ServiceKeys { get; }
    }

    public interface IServiceKeyContainer<TItem1, TItem2>
    {
        ServiceKeys<TItem1, TItem2> ServiceKeys { get; }
    }

    public interface IServiceKeyContainer<TItem>
    {
        ServiceKeys<TItem> ServiceKeys { get; }
    }
}