using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using RecipeShare.Web.Services;
using System.Security.Claims;

namespace RecipeShare.Web.Helpers.System
{
    public class RSComponentBase : ComponentBase
    {
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected SeedService _seedService { get; set; } = default!;

        //Seeded Logic Flag
        protected bool IsSeeded { get; private set; } = false;
        protected string? SeedResultMessage { get; private set; }

        protected bool IsLoggedIn { get; private set; } = false;
        protected ClaimsPrincipal User { get; private set; } = new ClaimsPrincipal();
        protected string? UserName => User.Identity?.Name;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
            IsLoggedIn = User.Identity?.IsAuthenticated ?? false;

            //Check the Seed state
            await CheckSeedStateAsync();
        }

        protected async Task CheckSeedStateAsync()
        {
            IsSeeded = await _seedService.IsSeededAsync();
        }

        protected async Task SeedDatabaseAsync()
        {
            //Seed the Data
            SeedResultMessage = await _seedService.SeedAsync();

            // Refresh local flag
            await CheckSeedStateAsync();

            // Optional: display result (console or toast)
            Console.WriteLine($"Seed result: {SeedResultMessage}");
            NavigationManager.NavigateTo("", forceLoad: true);
        }

        protected void LogoutAndRedirect()
        {
            NavigationManager.NavigateTo("Identity/Account/Logout", forceLoad: true);
        }
    }
}
