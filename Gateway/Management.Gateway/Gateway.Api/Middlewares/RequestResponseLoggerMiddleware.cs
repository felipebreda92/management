using System.Text;
using Gateway.Api.Extensions;
using Gateway.Application.Logging.Interfaces;
using Gateway.Infrastructure.Logging.Dtos;

namespace Gateway.Api.Middlewares;

public class RequestResponseLoggerMiddleware : IMiddleware
{
    private readonly ILoggingService _loggingService;

    public RequestResponseLoggerMiddleware(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Log de request
        string requestBody = "No Body for this request method.";
        
        if (context.Request.Method == HttpMethods.Post ||
            context.Request.Method == HttpMethods.Put ||
            context.Request.Method == HttpMethods.Patch)
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
        }
        
        await _loggingService.LogAsync(new LogEntry()
        {
            Type = "Request",
            Method = context.Request.Method,
            Path = context.Request.Path,
            Body = requestBody,
            Timestamp = DateTime.UtcNow,
        });
        
        // Capture Response
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        
        await next(context);
        
        //LOG RESPONSE
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
        
        await _loggingService.LogAsync(new LogEntry()
        {
            Type = "Response",
            Method = context.Request.Method,
            Path = context.Request.Path,
            Body = responseBodyText,
            Timestamp = DateTime.UtcNow,
            StatusCode = context.Response.StatusCode.ToString()
        });
        
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
        
        context.Response.Body = originalBodyStream;
    }
}