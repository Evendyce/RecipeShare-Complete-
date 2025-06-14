using RecipeShare.Web.Helpers.System;
using Microsoft.JSInterop;

namespace RecipeShare.Web.Components.Layout.Sections.Footer
{
    public class VgFooterBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        protected string _themeIcon = "fas fa-moon"; // default to dark

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                // 🔍 Check current theme from JS and update icon
                var theme = await JS.InvokeAsync<string>("document.documentElement.getAttribute", "data-theme");
                _themeIcon = theme == "light" ? "fas fa-sun" : "fas fa-moon";

                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task ToggleTheme()
        {
            await JS.InvokeVoidAsync("voidGlassTheme.toggle");

            // ⏱ Give the DOM a tick to update
            await Task.Delay(50);

            // 🔄 Read the new theme from <html>
            var theme = await JS.InvokeAsync<string>("document.documentElement.getAttribute", "data-theme");
            _themeIcon = theme == "light" ? "fas fa-sun" : "fas fa-moon";

            await InvokeAsync(StateHasChanged);
        }

    }
}
