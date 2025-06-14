using Microsoft.AspNetCore.Components;
using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.VoidGlass.VG2x2
{
    public class VG2x2Base : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        [Parameter] public string? CssClass { get; set; }
        [Parameter] public string? Style { get; set; }

        [Parameter] public string? TopLeftHeading { get; set; }
        [Parameter] public string? TopRightHeading { get; set; }
        [Parameter] public string? BottomLeftHeading { get; set; }
        [Parameter] public string? BottomRightHeading { get; set; }

        [Parameter] public RenderFragment? TopLeft { get; set; }
        [Parameter] public RenderFragment? TopRight { get; set; }
        [Parameter] public RenderFragment? BottomLeft { get; set; }
        [Parameter] public RenderFragment? BottomRight { get; set; }

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
