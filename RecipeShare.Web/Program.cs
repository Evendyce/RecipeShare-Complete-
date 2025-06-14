using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeShare.Web.Components;
using RecipeShare.Web.Components.Account;
using RecipeShare.Web.Components.Layout.Sections.Footer;
using RecipeShare.Web.Data;
using RecipeShare.Web.Services;

namespace RecipeShare.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------
            // Razor Components
            // -------------------------------
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // -------------------------------
            // Identity & Authentication
            // -------------------------------
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            // -------------------------------
            // Database Context
            // -------------------------------
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDbContextFactory<RecipeShareContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // -------------------------------
            // Custom Services
            // -------------------------------
            builder.Services.AddSingleton<SnapZoneConfig>();

            // Seed logic related services
            builder.Services.AddScoped<SeedService>();

            var app = builder.Build();

            // -------------------------------
            // Middleware & Pipeline
            // -------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // -------------------------------
            // Identity Endpoints
            // -------------------------------
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}