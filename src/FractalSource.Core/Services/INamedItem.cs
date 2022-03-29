namespace FractalSource.Services
{
    public interface INamedItem : IServiceItem
    {
        string Name { get; set; }
    }
}