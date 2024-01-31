using Microsoft.EntityFrameworkCore;
using OrderService.Extensions;
using OrderService.Data;
using OrderService.Services.IServices;
using OrderService.Services;
using AuctionGardeMessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrder, OrdersService>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IArt, ArtsService>();
builder.Services.AddScoped<IBid, BidsService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Service for connection to database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myconnection"));
});

//custom services
builder.AddAuth();
builder.AddSwaggenGenExtension();

builder.Services.AddHttpClient("Users", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:UserService")));
builder.Services.AddHttpClient("Arts", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:ArtService")));
builder.Services.AddHttpClient("Bids", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:BidService")));

var app = builder.Build();

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Stripe:Key");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
