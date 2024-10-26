using Ecom.Core.Events;
using Ecom.Data;
using Ecom.Services;
using Ecom.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Dependencies.ConfigureDatabase(builder.Configuration, builder.Services);
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("MyEshop")));

builder.Services.AddDistributedMemoryCache();


//Dependencies.ConfigureDatabase(builder.Configuration, builder.Services);

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));

builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddTransient<DbInitializer>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//app.ApplyMigrations();

SeedData.Populate(app);
app.Run();
