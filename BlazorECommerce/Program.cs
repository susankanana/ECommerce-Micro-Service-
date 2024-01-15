using BlazorECommerce;
using BlazorECommerce.Services;
using BlazorECommerce.Services.IService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<IAuthRegister,AuthServiceRegister>();
builder.Services.AddScoped<IAuthLogin,AuthServiceLogin>();
builder.Services.AddScoped<IProduct,ProductsService>();
builder.Services.AddScoped<ICoupon,CouponsService>();
builder.Services.AddScoped<ICart,CartsService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
