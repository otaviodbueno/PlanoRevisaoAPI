using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;


namespace PlanoRevisaoAPI;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // segue o pipeline normalmente
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // captura exceções não tratadas
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new
        {
            message = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsJsonAsync(response);
    }
}
