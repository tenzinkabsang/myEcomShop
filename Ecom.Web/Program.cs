using Ecom.Core.Events;
using Ecom.Data;
using Ecom.Web.Configuration;
using Ecom.Web.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureCache(builder.Configuration);
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)));
builder.Services.ConfigureHttpApiClients();


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

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
    };
});


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

app.Run();
