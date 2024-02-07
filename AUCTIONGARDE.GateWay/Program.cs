using AUCTIONGARDE.GateWay.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("ocelot.json", reloadOnChange: true, optional: false);
builder.Services.AddOcelot(builder.Configuration); //tells ocelot that the file it will be using will be given by builder.Configuration above

builder.AddAuth();

builder.Services.AddCors(options => options.AddPolicy("policy1", build =>
{
    build.AllowAnyOrigin();
    build.AllowAnyHeader();
    build.AllowAnyMethod();
}));
var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.UseCors("policy1");


app.UseOcelot().GetAwaiter().GetResult();
app.Run();


