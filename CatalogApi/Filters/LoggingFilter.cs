using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogApi.Filters;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("OnActionExecuting");
        _logger.LogInformation("############################");
        _logger.LogInformation($"{context.ModelState.IsValid}");
        _logger.LogInformation("############################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("OnActionExecuted");
        _logger.LogInformation("##########################");
        _logger.LogInformation($"{context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("##########################");
    }
}