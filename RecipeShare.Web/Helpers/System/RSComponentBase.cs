using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace RecipeShare.Web.Helpers.System
{
    public class RSComponentBase : ComponentBase
    {
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        protected bool IsLoggedIn { get; private set; } = false;
        protected ClaimsPrincipal User { get; private set; } = new ClaimsPrincipal();
        protected string? UserName => User.Identity?.Name;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
            IsLoggedIn = User.Identity?.IsAuthenticated ?? false;
        }

        protected void LogoutAndRedirect()
        {
            NavigationManager.NavigateTo("Identity/Account/Logout", forceLoad: true);
        }
    }
}
