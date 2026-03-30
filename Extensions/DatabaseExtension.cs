using Microsoft.EntityFrameworkCore;
using OliveOilEshop.Database;

namespace OliveOilEshop.Extensions
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
