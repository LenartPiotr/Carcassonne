using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Server.DatabaseContext;
using Server.Services.CarcassoneGame;
using Server.Services.HubSessionBridge;
using Server.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// FE core with build in memory databases
//builder.Services.AddDbContext<CarcassonneContext>(options =>
//    options.UseInMemoryDatabase("CarcassonneDatabase"));

// FE core with database

builder.Services.AddDbContext<AppDatabaseContext>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("Default") ?? ""));

builder.Services.AddControllersWithViews();

// Mysql connection
// builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Default")));

// Use session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Use signalR
builder.Services.AddSignalR();

// Add game service as singleton
builder.Services.AddSingleton<ICarcassonneGame, CarcassonneGame>();

// Add service connect session and hub
builder.Services.AddSingleton<IBridge, HSBridge>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// Use SignalR endpoints
app.MapHub<GlobalHub>("/hub");

app.MapFallbackToFile("index.html"); ;

app.Run();
