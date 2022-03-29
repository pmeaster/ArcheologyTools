namespace FractalSource.Services
{
    public abstract class NamedValueItem<TValue> : ValueItem<TValue>, INamedValueItem<TValue>
    {
        public string Name { get; set; }

        public override string ToString() => Value?.ToString() ?? base.ToString();
    }
}