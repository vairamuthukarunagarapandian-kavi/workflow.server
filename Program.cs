using Microsoft.Azure.SignalR;
using PiiSignalRDemo.Hubs;
using PiiSignalRDemo.Queue;
using PiiSignalRDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000, https://workflow-signalr.netlify.app/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});



builder.Services.AddControllers();

builder.Services.AddSignalR()
    .AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);

builder.Services.AddSingleton<PiiProcessorService>();

// Choose queue implementation

builder.Services.AddSingleton<IQueueService, LocalQueueService>();

// builder.Services.AddSingleton<IQueueService, AzureServiceBusQueueService>();

builder.Services.AddHostedService<BackgroundWorker>();

var app = builder.Build();

app.UseCors("ClientPolicy");

app.MapControllers();

app.MapHub<RuleHub>("/hub/rulehub");

app.Run();
