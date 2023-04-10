using ArdaManager.Server;
using ArdaManager.Server.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);
builder.Host.UseSerilog();
var app = builder.Build();
//app.UseCors("VappSpec");
app.UseDeveloperExceptionPage();

startup.Configure(app, app.Environment);



app.Run();