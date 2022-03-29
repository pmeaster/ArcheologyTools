namespace FractalSource.Services
{
    public abstract class NamedItem : ServiceItem, INamedItem
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name) 
                ? Name 
                : base.ToString();
        }
    }
}