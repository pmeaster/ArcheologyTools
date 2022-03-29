using System.Web;
using FractalSource.Services;

namespace FractalSource.Net.Endpoint
{
    public abstract class EndpointParameter<TValue> : NamedValueItem<TValue>, IEndpointParameter<TValue>
    {
        protected EndpointParameter(IEndpointParameterName parameterName, IValueItem<TValue> parameterValue)
        {
            Name = parameterName.Value;
            Value = parameterValue.Value;
        }

        protected EndpointParameter(IEndpointParameterName parameterName)
        {
            Name = parameterName.Value;
        }

        protected virtual string OnGetNameString()
        {
            return Name;
        }

        protected virtual string OnGetValueString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        protected virtual string OnGetParameterString()
        {
            var name = OnGetNameString();
            var value = HttpUtility.UrlEncode(OnGetValueString());

            return $"{name}={value}";
        }

        public override string ToString()
        {
            return OnGetParameterString();
        }

        public string GetParameterString()
        {
            return OnGetParameterString();
        }
    }
}