using Microsoft.AspNetCore.Components;
using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.VoidGlass.VGCard
{
    public class VGCardBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        [Parameter] public string? Header { get; set; }
        [Parameter] public string? SubHeader { get; set; }
        [Parameter] public string? Icon { get; set; }

        [Parameter] public RenderFragment? HeaderTemplate { get; set; }
        [Parameter] public RenderFragment? ContentTemplate { get; set; }
        [Parameter] public RenderFragment? FooterTemplate { get; set; }

        [Parameter] public string? Content { get; set; }
        [Parameter] public string? Footer { get; set; }

        [Parameter] public string? CssClass { get; set; }
        [Parameter] public string? Style { get; set; }

        [Parameter] public string? Width { get; set; }
        [Parameter] public string? Height { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                //{ Code here }

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
