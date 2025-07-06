using MiniTicaret.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace MiniTicaret.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // 🧠 Console-a yaz
            Console.WriteLine("🔥 EXCEPTION CAUGHT:");
            Console.WriteLine("Message => " + ex.Message);
            Console.WriteLine("StackTrace => " + ex.StackTrace);

            // 🧠 Log fayla yaz (ILogger ilə)
            _logger.LogError(ex, ex.Message);

            // ✳️ Status kodu müəyyən et
            int statusCode = (int)HttpStatusCode.InternalServerError;

            if (ex is AppException appEx)
            {
                statusCode = appEx.StatusCode; // Custom status code
            }
            else if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (ex is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }

            // ✳️ JSON response formalaşdır
            var response = new
            {
                StatusCode = statusCode,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}