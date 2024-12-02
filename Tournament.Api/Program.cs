using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Tournament.Api.Extensions;
using Tournament.Core.Repositories;
using Tournament.Data.Repositories;

namespace Tournament.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext with SQL Server
            builder.Services.AddDbContext<TournamentApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext")
                ?? throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //To make repositories work and function
            builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();

            // Add support for JSON and XML serialization in API responses
            //builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
            //    .AddNewtonsoftJson()
            //    .AddXmlDataContractSerializerFormatters();

            builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddXmlDataContractSerializerFormatters();


            var app = builder.Build();

            // Seed the database asynchronously
            await app.SeedDataAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Run the application
            await app.RunAsync();
        }
    }
}
