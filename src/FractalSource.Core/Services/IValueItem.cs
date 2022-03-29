namespace FractalSource.Services
{
    public interface IValueItem<TValue> : IServiceItem
    {
        TValue Value { get; set; }
    }
}