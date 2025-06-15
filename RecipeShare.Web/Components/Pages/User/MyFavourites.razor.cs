using Microsoft.AspNetCore.Components;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;

namespace RecipeShare.Web.Components.Pages.User
{
    public class MyFavouritesBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected IRecipeService _recipeService { get; set; }

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


                var filter = new RecipeSearchDto
                {
                    Username = UserName
                };

                recipes = await _recipeService.GetRecipeTilesForUserAsync(filter);

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task ToggleFavourite(RecipeTileDto recipe)
        {
            var dto = new FavouriteToggleRequestDto
            {
                RecipeId = recipe.Id,
                Username = UserName!
            };

            var success = await _recipeService.ToggleFavouriteAsync(dto);

            if (success)
            {
                recipe.IsFavouritedByUser = !recipe.IsFavouritedByUser;
                recipe.FavouriteCount += recipe.IsFavouritedByUser ? 1 : -1;

                // Optionally remove unfavourited from view
                if (!recipe.IsFavouritedByUser)
                    recipes!.Remove(recipe);
            }
        }
    }
}
