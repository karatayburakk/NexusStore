using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NexusStore.API.Middleware
{
  public class JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
  {
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger _logger = logger;

    public async Task Invoke(HttpContext context)
    {
      var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

      if (token != null)
      {
        AttachUserToContext(context, token);
      }

      await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(token)));
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = true,
          ValidIssuer = _configuration["Jwt:Issuer"],
          ValidateAudience = true,
          ValidAudience = _configuration["Jwt:Audience"],
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

        // Attach user to context on successful JWT validation
        context.Items["User"] = userId;
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "JWT validation failed.");
        // Do nothing if JWT validation fails
        // User is not attached to context so request won't have access to secure routes
      }
    }
  }
}