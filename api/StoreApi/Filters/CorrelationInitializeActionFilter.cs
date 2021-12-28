using System.Security.Claims;
using Store.Service.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreApi.Filters;

public class CorrelationInitializeActionFilter : IActionFilter
{
    private readonly ICorrelationService _correlationService;

    public CorrelationInitializeActionFilter(ICorrelationService correlationService)
    {
        _correlationService = correlationService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = int.TryParse(context?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userInt) ? userInt : null;
        _correlationService.SetUserId(userId);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}