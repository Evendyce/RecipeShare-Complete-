using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using RecipeShare.Models.Web;
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

            var env = builder.Environment;

            // -------------------------------
            // Add CORS Policy for API Domain expansion
            // -------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

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
            // AppSettings binding and HttpClient setup
            // -------------------------------
            // Register full AppSettings binding (if you want access to all sections)
            //builder.Services.Configure<AppSettingsDto>(builder.Configuration);

            // OR register just the API section if that's all you need
            builder.Services.Configure<ApiSettingsDto>(builder.Configuration.GetSection("Api"));

            // Configure HttpClient using strongly typed settings
            builder.Services.AddScoped(sp =>
            {
                var apiOptions = sp.GetRequiredService<IOptions<ApiSettingsDto>>();
                var baseUrl = apiOptions.Value.BaseUrl;

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("API BaseUrl is not configured.");

                return new HttpClient { BaseAddress = new Uri(baseUrl) };
            });

            // -------------------------------
            // Custom Services
            // -------------------------------
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IImageUploadService, ImageUploadService>();

            builder.Services.AddSingleton<SnapZoneConfig>();

            // Seed logic related services
            builder.Services.AddScoped<SeedService>();

            var app = builder.Build();

            app.UseStaticFiles(); // for wwwroot

            // 👇 serves /uploads mapped to wwwroot/uploads
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = "/uploads"
            });

            // -------------------------------
            // Middleware & Pipeline
            // -------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseCors();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseCors();
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