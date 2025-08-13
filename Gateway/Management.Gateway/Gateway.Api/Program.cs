using Gateway.Api.Middlewares;
using Gateway.Application.Logging;
using Gateway.Application.Logging.Interfaces;
using Gateway.Infrastructure.Configurations;
using Gateway.Infrastructure.Logging;
using Gateway.Infrastructure.Logging.Interfaces;
using Gateway.Infrastructure.Repositories;
using Gateway.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("Logging:Mongo"));


builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddScoped<ILogRepository, MongoLogRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<RequestResponseLoggerMiddleware>();

var app = builder.Build();
app.UseMiddleware<RequestResponseLoggerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();