using Ecom.Catalog.Api;
using Ecom.Catalog.Api.Services;
using Ecom.Core.Events;
using Ecom.Core.Middlewares;
using Ecom.Data;
using Ecom.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ecom.Catalog.Api.Configuration;
using Ecom.Core.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    var connectionString = builder.Configuration[builder.Configuration["AZURE_SQL_CONNECTION_STRING"] ?? "ConnectionStrings:MyEshop"];
    opt.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.ConfigureCache(builder.Configuration);

builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));
builder.Services.AddSingleton<IStaticCacheManager, HybridCacheManager>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped(typeof(IConsumer<>), typeof(CacheEventConsumer<>));


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

//app.UseAuthorization();

app.MapControllers();

SeedData.Populate(app);

app.MapHealthChecks("/health");

app.Run();
