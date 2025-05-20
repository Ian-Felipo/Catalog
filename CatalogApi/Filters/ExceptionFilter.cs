using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {

        _logger.LogError(context.Exception, "Ocorreu um exceção não tratada: Status Code 500");

        context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação: Status Code 500")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}
