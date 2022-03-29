using Microsoft.AspNetCore.Mvc;

namespace FractalSource.Mapping.Web.Controllers;

public abstract class MappingController<TController> : ControllerBase
    where TController : ControllerBase
{
    protected MappingController(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger<TController>();
        InstanceId = Guid.NewGuid();
    }

    protected ILogger<TController> Logger { get; }

    public virtual Guid InstanceId { get; }
}