using Microsoft.AspNetCore.Components;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;

namespace RecipeShare.Web.Components.Pages.User
{
    public class AddEditRecipeBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected IRecipeService _recipeService { get; set; }
        [Inject]
        protected NavigationManager Nav { get; set; }

        [Parameter] public long? RecipeId { get; set; }

        protected bool IsEditMode => RecipeId.HasValue;

        protected RecipeDto CurrentRecipe { get; set; } = new();
        protected RecipeFormBase? RecipeFormRef;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                if (IsEditMode)
                {
                    await GetRecipe();
                }

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task GetRecipe()
        {
            CurrentRecipe = await _recipeService.GetRecipeByIdAsync(RecipeId.Value, "");
        }

        protected async Task HandleSubmit(RecipeDto model)
        {
            model.DietaryTags = String.Join(", ", model.Tags.Select(x => x.Name));
            model.Steps = String.Join(", ", model.StructuredSteps.Select(x => x.Instruction));

            if (IsEditMode)
            {
                model.UserName = UserName;
                await _recipeService.UpdateRecipeAsync(model);
            }
            else
            {
                model.UserName = UserName;
                var newRecipeId = await _recipeService.CreateRecipeAsync(model);

                // Finalize image uploads
                if (RecipeFormRef is not null)
                    await RecipeFormRef.FinalizeImageUploadAsync(newRecipeId);

                // Optional: persist changes to DB after updating paths
                await _recipeService.UpdateRecipeAsync(model);
            }

            Nav.NavigateTo("/my-recipes");
        }
    }
}
