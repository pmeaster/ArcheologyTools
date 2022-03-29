

// ReSharper disable StaticMemberInGenericType

namespace FractalSource.Services
{
    public class ValueItem<TValue> : ServiceItem, IValueItem<TValue>
    {
        public TValue Value { get; set; }

        public override string ToString() => (Value?.ToString() ?? base.ToString()) ?? string.Empty;

        public static implicit operator TValue(ValueItem<TValue> valueItem) => valueItem.Value;

        public static implicit operator ValueItem<TValue>(TValue value) => new()
        {
            Value = value
        };
    }
}