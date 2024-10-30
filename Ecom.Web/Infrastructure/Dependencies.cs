using Ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Web.Infrastructure;

public static class Dependencies
{
    public static void ConfigureDatabase(IConfiguration configuration, IServiceCollection services)
    {
        bool useInMemoryDb = false;
        if (configuration["UseInMemoryDb"] != null)
        {
            useInMemoryDb = bool.Parse(configuration["UseInMemoryDb"]!);
        }

        if(useInMemoryDb)
        {
            services.AddDbContext<ApplicationDbContext>(c => c.UseInMemoryDatabase("MyEshop"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                var connectionString = configuration[configuration["AZURE_SQL_CONNECTION_STRING"] ?? "ConnectionStrings:MyEshop"];
                opt.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
            });
        }
    }
}