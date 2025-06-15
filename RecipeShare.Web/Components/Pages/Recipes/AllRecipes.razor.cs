using Microsoft.AspNetCore.Components;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;
using System.IO;
using static System.Net.WebRequestMethods;

namespace RecipeShare.Web.Components.Pages.Recipes
{
    public class AllRecipesBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected IRecipeService _recipeService { get; set; }
        [Inject]
        protected NavigationManager Nav {  get; set; }

        protected List<RecipeTileDto>? recipes;
        protected RecipeSearchDto filter = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                await LoadRecipes();

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task LoadRecipes()
        {
            filter.Username = UserName;
            recipes = await _recipeService.GetRecipeTilesAsync(filter);
        }

        protected async Task OnFilterChanged(RecipeSearchDto updatedFilter)
        {
            filter = updatedFilter;
            await LoadRecipes();
        }
        protected async Task ToggleFavourite(RecipeTileDto recipe)
        {
            // Call API to toggle the state
            recipe.IsFavouritedByUser = !recipe.IsFavouritedByUser;

            // Fake optimistic update (replace with actual result if needed)
            recipe.FavouriteCount += recipe.IsFavouritedByUser ? 1 : -1;

            var dto = new FavouriteToggleRequestDto
            {
                RecipeId = recipe.Id,
                Username = UserName!
            };

            await _recipeService.ToggleFavouriteAsync(dto);
        }
    }
}
