using cpu_process_api;
using cpu_process_api.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000","https://localhost:3000","http://localhost/", "https://localhost/")
            .AllowCredentials();
    });
}); 
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("ClientPermission");
app.MapHub<ProcessHub>("/process");
app.UseRouting();
app.MapControllers();
app.Run();

