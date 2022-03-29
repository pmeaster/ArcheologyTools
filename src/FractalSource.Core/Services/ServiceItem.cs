using System;

namespace FractalSource.Services
{
    public abstract class ServiceItem : IServiceItem
    {
        protected ServiceItem()
        {
            InstanceId = Guid.NewGuid();
        }

        public virtual Guid InstanceId { get; }
    }
}