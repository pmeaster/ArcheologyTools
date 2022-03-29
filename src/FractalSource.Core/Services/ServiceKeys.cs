// ReSharper disable UnusedTypeParameter

namespace FractalSource.Services
{
    public readonly struct ServiceKeys<TItem1, TItem2, TItem3, TItem4> : IServiceKeys
    {
        public ServiceKey<TItem3, TItem4> AddressKey => default;

        public ServiceKey<TItem3> DescriptionKey => default;

        public ServiceKey<TItem1, TItem2> RecordHandlerKey => default;

        public ServiceKey<TItem1, TItem3, TItem4> EndpointProviderKey => default;

        public ServiceKey<TItem2> EndpointHandlerKey => default;

        public ServiceKey<TItem1, TItem3, TItem4> EndpointKey => default;

        public ServiceKey<TItem1> ItemKey => default;
    }

    public readonly struct ServiceKeys<TItem1, TItem2, TItem3> : IServiceKeys
    {
        public ServiceKey<TItem2, TItem3> AddressKey => default;

        public ServiceKey<TItem2> DescriptionKey => default;

        public ServiceKey<TItem1, TItem2, TItem3> RecordProviderKey => default;

        public ServiceKey<TItem1, TItem2, TItem3> StreamHandlerKey => default;

        public ServiceKey<TItem1, TItem2, TItem3> EndpointKey => default;

        public ServiceKey<TItem1> ItemKey => default;
    }

    public readonly struct ServiceKeys<TItem1, TItem2> : IServiceKeys<TItem1, TItem2>
    {
        public ServiceKey<TItem1, TItem2> RecordConverterKey => default;

        public ServiceKey<TItem1, TItem2> RecordProviderKey => default;

        public ServiceKey<TItem1> ItemKey => default;
    }

    public readonly struct ServiceKeys<TItem> : IServiceKeys<TItem>
    {
        public ServiceKey<TItem> ItemKey => default;
    }
}