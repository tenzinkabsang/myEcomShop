using Ecom.Core.Events;
using Ecom.Data;
using Ecom.Orders.Api.Endpoints.Checkout;
using Ecom.Orders.Api.Endpoints.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

var connectionString = builder.Configuration[builder.Configuration["AZURE_SQL_CONNECTION_STRING"] ?? "ConnectionStrings:MyEshop"];

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

var redisConnStr = builder.Configuration[builder.Configuration["AZURE_REDIS_CONNECTION_STRING"] ?? "ConnectionStrings:Redis"];
if (!string.IsNullOrWhiteSpace(connectionString))
    builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisConnStr);
else
    builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped(sp => new FunDapperRepository(connectionString!));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapShoppingCartEndpoints();
app.MapCheckoutEndpoints();
app.Run();
