using System.Security.Claims;
using Store.Service.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreApi.Filters;

public class CorrelationInitializeActionFilter : IActionFilter
{
    private readonly ICorrelationService _correlationService;
    private readonly ILogger<CorrelationInitializeActionFilter> _logger;

    public CorrelationInitializeActionFilter(ICorrelationService correlationService, ILogger<CorrelationInitializeActionFilter> logger)
    {
        _correlationService = correlationService;
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            var doubleId = Convert.ToDouble(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int userId = Convert.ToInt32(doubleId);
            if (userId != 0)
            {
                _correlationService.SetUserId(userId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while parse userID.");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}