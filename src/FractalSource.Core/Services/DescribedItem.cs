namespace FractalSource.Services
{
    public abstract class DescribedItem : NamedItem, IDescribedItem
    {
        public string Description { get; set; }
    }
}