using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;

namespace RecipeShare.Web.Components.Pages.User
{
    public class MyRecipesBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected IRecipeService _recipeService { get; set; }
        [Inject]
        protected NavigationManager Nav { get; set; }

        protected List<RecipeTileDto>? recipes { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!IsLoggedIn) return;

                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                await LoadMyRecipes();

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task LoadMyRecipes()
        {
            var filter = new RecipeSearchDto
            {
                Username = UserName
            };

            recipes = await _recipeService.GetMyRecipeTilesAsync(filter);
        }

        protected async Task ConfirmDelete(long recipeId)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this recipe?");
            if (!confirmed) return;

            try
            {
                //await _recipeService.DeleteRecipeAsync(recipeId);
                var recipeToDelete = recipes.FirstOrDefault(x => x.Id == recipeId);
                if (recipeToDelete != null)
                {
                    recipes.Remove(recipeToDelete);
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "An error occurred while deleting the recipe.");
            }
        }
    }
}
