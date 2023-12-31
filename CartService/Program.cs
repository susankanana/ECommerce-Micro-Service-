using CartService.Data;
using CartService.Extensions;
using CartService.Services;
using CartService.Services.IServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICoupon, CouponService>();
builder.Services.AddScoped<IProduct, ProductsService>();
builder.Services.AddScoped<ICart, CartsService>();
builder.Services.AddScoped<ICartItem, CartItemService>();

builder.Services.AddHttpClient("Coupons", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:CouponService")));
builder.Services.AddHttpClient("Products", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceURl:ProductService")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Service for connection to database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myconnection"));
});

//custom services
builder.AddAuth();
builder.AddSwaggenGenExtension();

var app = builder.Build();

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
