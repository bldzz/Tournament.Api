using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<TournamentApiContext>();

                // Make sure the database is created.
                await context.Database.MigrateAsync(); // Use MigrateAsync instead of EnsureCreated for better management

                // Seed the data
                await SeedData.SeedAsync(context);
            }
            catch (Exception ex)
            {
                // Updated logger type to be more appropriate
                var logger = services.GetRequiredService<ILogger<object>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
