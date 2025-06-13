using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using FactoryPulse_Core.Data;
using FactoryPulse_Core.Hubs;
using FactoryPulse_Core.Services;
using FactoryPulse_Core.Endpoints;
using Serilog;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app-logs/factorypulse-log.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSignalR();
builder.Services.AddSingleton<MqttService>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(
    options => options
        .WithOrigins("http://localhost:3000") 
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);
app.MapHub<SensorHub>("/sensorHub");

var mqttService = app.Services.GetRequiredService<MqttService>();
await mqttService.StartAsync();

app.MapSensorEndpoints();

app.Run();