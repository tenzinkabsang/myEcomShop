using Ecom.Core.Events;
using Ecom.Data;
using Ecom.Services;
using Ecom.Services.Interfaces;
using Ecom.Web.Infrastructure;
using Ecom.Web.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

Dependencies.ConfigureDatabase(builder.Configuration, builder.Services);

var connectionString = builder.Configuration[builder.Configuration["AZURE_REDIS_CONNECTION_STRING"] ?? "ConnectionStrings:Redis"];

if(!string.IsNullOrWhiteSpace(connectionString))
    builder.Services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
else
    builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(TestUser.GetTestUser);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSession();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*");
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapStaticAssets();
app.UseSession();
app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "catpage",
    pattern: "{category}/Page{productPage:int}",
    new { Controller = "Home", action = "Index" }
    );

app.MapControllerRoute(
    name: "page",
    pattern: "Page{productPage:int}",
    new { Controller = "Home", action = "Index", productPage = 1 }
    );

app.MapControllerRoute(
    name: "category",
    pattern: "{category}",
    new { Controller = "Home", action = "Index", productPage = 1 }
    );

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1 }
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

SeedData.Populate(app);
app.Run();
