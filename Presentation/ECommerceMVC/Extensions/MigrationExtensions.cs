using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app) 
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ECommerceDbContext dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

            dbContext.Database.Migrate();

        }
    }
}
