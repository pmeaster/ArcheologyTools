using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public abstract class Service<TItem1, TItem2, TItem3, TItem4> : Service, IService<TItem1, TItem2, TItem3, TItem4>

    {
        protected Service(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public ServiceKeys<TItem1, TItem2, TItem3, TItem4> ServiceKeys => default;
    }

    public abstract class Service<TItem1, TItem2, TItem3> : Service, IService<TItem1, TItem2, TItem3>

    {
        protected Service(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public ServiceKeys<TItem1, TItem2, TItem3> ServiceKeys => default;
    }

    public abstract class Service<TItem1, TItem2> : Service, IService<TItem1, TItem2>

    {
        protected Service(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public ServiceKeys<TItem1, TItem2> ServiceKeys => default;
    }

    public abstract class Service<TItem> : Service, IService<TItem>

    {
        protected Service(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public ServiceKeys<TItem> ServiceKeys => default;
    }

    public abstract class Service : ServiceItem, IService

    {
        protected Service(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public ILogger Logger { get; }
    }
}