using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Server.DatabaseContext;
using Server.Services.CarcassoneGame;
using Server.Services.HubSessionBridge;
using Server.Services.Kafka;
using Server.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

// -- DATABASE INFO -- //

// If you want to use build with memory database uncomment line below and comment next one.

// FE core with build in memory databases
//builder.Services.AddDbContext<CarcassonneContext>(options =>
//    options.UseInMemoryDatabase("CarcassonneDatabase"));

// FE core with database connecting with Docker
builder.Services.AddDbContext<AppDatabaseContext>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("Default") ?? ""));

builder.Services.AddControllersWithViews();

// Use swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Use signalR
builder.Services.AddSignalR();

// Add game service as singleton
builder.Services.AddSingleton<ICarcassonneGame, CarcassonneGame>();

// Add service connect session and hub
builder.Services.AddSingleton<IBridge, HSBridge>();

// Add kafaka service
builder.Services.AddHostedService<KafkaConsumerHostedService>();
builder.Services.AddHostedService<KafkaProducerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

// Use Controllers endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// Use SignalR endpoints
app.MapHub<GlobalHub>("/hub");

app.MapFallbackToFile("index.html"); ;

app.Run();
