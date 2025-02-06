using System.Net;
using System.Text.Json;
using System.Data.Common; // added for DbException
// using Npgsql; // optionally if using Npgsql-specific exceptions
using NexusStore.API.Utilities;

namespace NexusStore.API.Middleware
{
  public class ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
  {

    private readonly RequestDelegate _next = next;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";

      context.Response.StatusCode = exception switch
      {
        CacheException => (int)HttpStatusCode.ServiceUnavailable,
        TimeoutException => (int)HttpStatusCode.GatewayTimeout,
        DbException => (int)HttpStatusCode.ServiceUnavailable,
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
        ArgumentException => (int)HttpStatusCode.BadRequest,
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        System.ComponentModel.DataAnnotations.ValidationException => (int)HttpStatusCode.BadRequest,
        Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,
        _ => (int)HttpStatusCode.InternalServerError
      };

      var errorDetails = new ErrorDetails
      {
        StatusCode = context.Response.StatusCode,
        Message = exception.Message,
        Details = _env.IsDevelopment() ? exception.StackTrace ?? string.Empty : null
      };

      var json = JsonSerializer.Serialize(errorDetails, JsonOptions.Options);
      await context.Response.WriteAsync(json);
    }
  }

  public class ErrorDetails
  {
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
  }

  public class CacheException : Exception
  {
    public CacheException(string message) : base(message) { }
    public CacheException(string message, Exception innerException) : base(message, innerException) { }
  }
}
