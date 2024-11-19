using System.Reflection;
using Ecom.Core.Caching;
using Ecom.Core.Events;
using Ecom.Core.Middlewares;
using Ecom.Data;
using Ecom.Orders.Api.Configuration;
using Ecom.Orders.Api.Endpoints.Checkout;
using Ecom.Orders.Api.Endpoints.ShoppingCart;
using Ecom.Orders.Api.Services;
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


builder.Services.ConfigureCache(builder.Configuration);
builder.Services.AddSingleton<IStaticCacheManager, HybridCacheManager>();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped(sp => new FunDapperRepository(connectionString!));
builder.Services.AddSingleton<IOrderPlacedEventPublisher, OrderPlacedEventPublisher>();

////event consumers
//var temp = typeof(Program).Assembly;
//var consumers = typeof(Program).Assembly.FindClassesOfType(typeof(IConsumer<>)).ToList();
//foreach (var consumer in consumers)
//    foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
//    {
//        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
//        return isMatch;
//    }, typeof(IConsumer<>)))
//        services.AddScoped(findInterface, consumer);


var app = builder.Build();
app.UseMiddleware<CustomExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
    };
});

app.MapShoppingCartEndpoints();
app.MapCheckoutEndpoints();
app.Run();
