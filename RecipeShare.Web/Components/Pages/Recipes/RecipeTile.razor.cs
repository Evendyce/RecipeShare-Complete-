using Microsoft.AspNetCore.Components;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.Pages.Recipes
{
    public class RecipeTileBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected NavigationManager _navMan { get; set; }

        [Parameter] public RecipeTileDto Recipe { get; set; } = new();

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

        protected void NavigateToRecipe(long id)
        {
            _navMan.NavigateTo($"/recipes/{id}");
        }
    }
}
