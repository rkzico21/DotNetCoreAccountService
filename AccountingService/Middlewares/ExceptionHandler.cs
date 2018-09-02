using System;
using System.Net;
using System.Threading.Tasks;
using AccountingService.Exceptions;
using AccountingService.Filetes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountingService.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;

        private readonly ILogger logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        
        if(exception is ResourceNotFoundException)     
        {
            code = HttpStatusCode.NotFound;
        }
        
        logger.LogError(exception, exception.Message); 
        
        var message = code == HttpStatusCode.InternalServerError ? "Internal Server Error" : exception.Message;
        var result = JsonConvert.SerializeObject(new  ApiErrorMessage(message));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
        }
    }
}