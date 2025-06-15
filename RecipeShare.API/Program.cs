
using Microsoft.EntityFrameworkCore;
using RecipeShare.API.Data;
using RecipeShare.API.Services;

namespace RecipeShare.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------
            // Database Context
            // -------------------------------
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<RecipeShareContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            // Custom Services
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<ITagService, TagService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // For realworld scenarios, uncomment the below
            // if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }

            // Comment this out for realworld
            app.MapOpenApi();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
