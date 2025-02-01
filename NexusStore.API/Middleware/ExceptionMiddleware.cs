using System.Net;
using System.Text.Json;

namespace NexusStore.API.Middleware
{
  public class ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
  // ILogger<ExceptionMiddleware> logger,
  {
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };

    private readonly RequestDelegate _next = next;
    // private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        // _logger.LogError(ex, "An unhandled exception occurred during request processing");
        await HandleExceptionAsync(context, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";

      context.Response.StatusCode = exception switch
      {
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
        ArgumentException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError
      };

      var errorDetails = new ErrorDetails
      {
        StatusCode = context.Response.StatusCode,
        Message = exception.Message,
        Details = _env.IsDevelopment() ? exception.StackTrace ?? string.Empty : null
      };

      var json = JsonSerializer.Serialize(errorDetails, SerializerOptions);
      await context.Response.WriteAsync(json);
    }
  }

  public class ErrorDetails
  {
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
  }
}
