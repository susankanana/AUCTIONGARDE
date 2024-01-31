using BidService.Data;
using BidService.Extensions;
using BidService.Services;
using BidService.Services.Iservices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IArt, ArtsService>();
builder.Services.AddScoped<IBid, BidsService>(); 
builder.Services.AddHttpClient("Arts", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:ArtService")));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Service for connection to database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myconnection"));
});

//custom services
builder.AddAuth();
builder.AddSwaggenGenExtension();

builder.Services.AddCors(options => options.AddPolicy("policy1", build =>
{
    build.AllowAnyOrigin();
    build.AllowAnyHeader();
    build.AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("policy1");

app.UseMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
