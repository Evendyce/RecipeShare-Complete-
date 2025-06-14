using Microsoft.AspNetCore.Components;
using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.Layout.Sections.TopBar
{
    public class VGTopBarBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

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
