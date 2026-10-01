using System.Net;
using System.Text.Json;
using SAVi.Application.Common.Exceptions;
using SAVi.Domain.Exceptions;
using ValidationException = SAVi.Application.Common.Exceptions.ValidationException;

namespace SAVi.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, titulo, erros) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest, "Erro de validação", (object?)validationException.Erros),
            NotFoundException => (HttpStatusCode.NotFound, exception.Message, null),
            AuthenticationFailedException => (HttpStatusCode.Unauthorized, exception.Message, null),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.", null)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Erro não tratado ao processar {Path}", context.Request.Path);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            titulo,
            status = (int)statusCode,
            erros
        }));
    }
}
