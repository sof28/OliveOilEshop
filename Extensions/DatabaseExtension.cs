using Microsoft.EntityFrameworkCore;
using ModularCommerce.Database;

namespace ModularCommerce.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddSQLDatabaseConfiguration(this IServiceCollection services,
          IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}

