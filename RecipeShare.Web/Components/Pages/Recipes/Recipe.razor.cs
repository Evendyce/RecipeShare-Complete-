using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;
using System.Text;

namespace RecipeShare.Web.Components.Pages.Recipes
{
    public class RecipeBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected IRecipeService _recipeService { get; set; }
        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        [Parameter] public long Id { get; set; }

        protected RecipeDto? recipe { get; set; }
        protected List<bool> _checkedSteps = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                recipe = await _recipeService.GetRecipeByIdAsync(Id, UserName);

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task ToggleFavourite()
        {
            if (recipe is null || !IsLoggedIn) return;

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
            }
        }

        protected void ResetAllSteps()
        {
            foreach (var step in recipe.StructuredSteps)
            {
                step.IsChecked = false;
            }
        }

        protected async Task ExportRecipe()
        {
            var markdown = GenerateMarkdown();
            var filename = $"{recipe.Title.Replace(" ", "_")}.md";

            var title = recipe.Title.Replace(" ", "_");


            //-- Choose which Export --
            //PDF Export
            await JS.InvokeVoidAsync("exportMarkdownAsPdf", markdown, title);

            //Markdown Export
            //await JS.InvokeVoidAsync("downloadFile", filename, markdown);
        }

        private string GenerateMarkdown()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"# {recipe.Title}");
            sb.AppendLine();
            sb.AppendLine($"**Cooking Time:** {recipe.CookingTime} minutes");
            sb.AppendLine();

            if (recipe.Tags?.Any() == true)
            {
                sb.AppendLine($"**Tags:** {string.Join(", ", recipe.Tags.Select(t => t.Name))}");
                sb.AppendLine();
            }

            sb.AppendLine("## Ingredients");
            sb.AppendLine(recipe.Ingredients);
            sb.AppendLine();

            sb.AppendLine("## Steps");

            if (recipe.StructuredSteps?.Any() == true)
            {
                var count = 1;
                foreach (var step in recipe.StructuredSteps.OrderBy(s => s.Order))
                {
                    sb.AppendLine($"- [ ] {step.Instruction}");
                    count++;
                }
            }
            else
            {
                sb.AppendLine(recipe.Steps);
            }

            return sb.ToString();
        }

    }
}
