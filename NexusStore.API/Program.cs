using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;
using NexusStore.API.Mappings;
using NexusStore.API.Middleware;
using NexusStore.API.Repositories;
using NexusStore.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddControllers();

// Uncomment the following line if you want to use Swagger for API documentation
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Uncomment the following lines if you want to use Swagger for API documentation
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
