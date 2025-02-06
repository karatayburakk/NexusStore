using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;
using NexusStore.API.Mappings;
using NexusStore.API.Middleware;
using NexusStore.API.Repositories;
using NexusStore.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using System.Text.Json;
using Hangfire.PostgreSql;
using NexusStore.API.Jobs;
using NexusStore.API.Utilities;

namespace NexusStore.API
{
  public class Startup(IConfiguration configuration)
  {
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<NexusDbContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = Configuration["Jwt:Issuer"],
              ValidAudience = Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(services))))
            };

            options.Events = new JwtBearerEvents
            {
              OnChallenge = async context =>
              {
                // Skip the default logic.
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var errorDetails = new
                {
                  StatusCode = StatusCodes.Status401Unauthorized,
                  Message = "Unauthorized",
                  Details = "Authentication failed. Please provide valid credentials."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails, JsonOptions.Options));
              }
            };
          });

      // Uncomment the following lines if you want to use Redis for caching
      // services.AddStackExchangeRedisCache(options =>
      // {
      //   options.Configuration = Configuration.GetConnectionString("Redis");
      //   options.InstanceName = "NexusStore:";
      // });

      services.AddEnyimMemcached(Configuration.GetSection("enyimMemcached"));

      services.AddAuthorization();

      services.AddEndpointsApiExplorer();

      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<ICategoryRepository, CategoryRepository>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IDataService, DataService>();
      services.AddScoped<ICountryRepository, CountryRepository>();

      services.AddAutoMapper(typeof(UserProfile), typeof(CategoryProfile), typeof(CountryProfile));

      services.AddControllers();

      services.AddHangfire(config =>
      {
        config.UsePostgreSqlStorage(options =>
        {
          options.UseNpgsqlConnection(Configuration.GetConnectionString("HangfireConnection"));
        });
      });
      services.AddHangfireServer();

      // Uncomment the following line if you want to use Swagger for API documentation
      // services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        // Uncomment the following lines if you want to use Swagger for API documentation
        // app.UseSwagger();
        // app.UseSwaggerUI();
        Console.WriteLine("Development environment");
      }

      app.UseMiddleware<ExceptionMiddleware>(); // Place ExceptionMiddleware at the beginning

      app.UseEnyimMemcached();
      app.UseRouting();

      app.UseMiddleware<JwtMiddleware>();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHangfireDashboard("/hangfire"); // Ensure the dashboard is accessible at /hangfire

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHangfireDashboard("/hangfire"); // Ensure the dashboard is mapped correctly
      });

      // Schedule the recurring jobs
      JobScheduler.ScheduleJobs();
    }
  }
}