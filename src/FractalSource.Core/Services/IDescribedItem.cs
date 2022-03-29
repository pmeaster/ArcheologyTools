namespace FractalSource.Services
{
    public interface IDescribedItem : INamedItem
    {
        string Description { get; set; }
    }
}