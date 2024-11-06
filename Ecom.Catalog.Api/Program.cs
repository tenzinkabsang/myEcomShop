﻿using Ecom.Catalog.Api;
using Ecom.Catalog.Api.Services;
using Ecom.Core.Events;
using Ecom.Data;
using Ecom.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    var connectionString = builder.Configuration[builder.Configuration["AZURE_SQL_CONNECTION_STRING"] ?? "ConnectionStrings:MyEshop"];
    opt.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

var connectionString = builder.Configuration[builder.Configuration["AZURE_REDIS_CONNECTION_STRING"] ?? "ConnectionStrings:Redis"];

if (!string.IsNullOrWhiteSpace(connectionString))
    builder.Services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
else
    builder.Services.AddDistributedMemoryCache();


builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

SeedData.Populate(app);
app.Run();
