using System.Security.Claims;
using AirSoft.Service.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AirSoftApi.Filters;

public class CorrelationInitializeActionFilter : IActionFilter
{
    private readonly ICorrelationService _correlationService;

    public CorrelationInitializeActionFilter(ICorrelationService correlationService)
    {
        _correlationService = correlationService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Guid? userId = Guid.TryParse(context?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userGuid) ? userGuid : null;
        _correlationService.SetUserId(userId);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}