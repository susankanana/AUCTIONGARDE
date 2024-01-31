using AUCTIONGARDE_Frontend;
using AUCTIONGARDE_Frontend.Services.ArtS;
using AUCTIONGARDE_Frontend.Services.Auth;
using AUCTIONGARDE_Frontend.Services.AuthProv;
using AUCTIONGARDE_Frontend.Services.BidS;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthRegister, AuthServiceRegister>();
builder.Services.AddScoped<IAuthLogin, AuthServiceLogin>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IArt, ArtService>();
builder.Services.AddScoped<IBid, BidService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Configuration for AuthProvider
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();

await builder.Build().RunAsync();
