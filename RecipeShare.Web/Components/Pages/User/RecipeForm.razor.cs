using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;

namespace RecipeShare.Web.Components.Pages.User
{
    public class RecipeFormBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        // TEMPORARY GUID for Pre-Creation Uploads
        protected Guid TempReferenceId { get; set; } = Guid.NewGuid();

        //I N J E C T I O N S
        [Inject] protected ITagService _tagService { get; set; }
        [Inject] protected IImageUploadService _uploadService { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }

        [Parameter] public RecipeDto Recipe { get; set; } = new();
        [Parameter] public EventCallback<RecipeDto> OnSubmit { get; set; }
        [Parameter] public bool IsEditMode { get; set; }

        protected List<TagDto> AllTags = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                AllTags = await _tagService.GetAllTagsAsync();

                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected void ToggleTag(TagDto tag)
        {
            var match = Recipe.Tags.FirstOrDefault(t => t.Id == tag.Id);
            if (match != null)
                Recipe.Tags.Remove(match);
            else
                Recipe.Tags.Add(tag);
        }

        protected void AddStep()
        {
            Recipe.StructuredSteps.Add(new RecipeStepDto
            {
                Order = Recipe.StructuredSteps.Count + 1
            });
        }

        protected void RemoveStep(RecipeStepDto step)
        {
            Recipe.StructuredSteps.Remove(step);
            for (int i = 0; i < Recipe.StructuredSteps.Count; i++)
                Recipe.StructuredSteps[i].Order = i + 1;
        }

        protected async Task RemoveImageAsync(RecipeImageDto image)
        {
            if (!string.IsNullOrWhiteSpace(image.ImageUrl))
            {
                await _uploadService.DeleteAsync(image.ImageUrl);
            }

            Recipe.Images.Remove(image);
        }

        protected async Task HandleImageUpload(InputFileChangeEventArgs e, bool isCover)
        {
            var file = e.File;
            string url;

            if (IsEditMode && Recipe.Id > 0)
            {
                url = await _uploadService.SaveAsync(file, Recipe.Id, isCover);
            }
            else
            {
                url = await _uploadService.SaveTempAsync(file, TempReferenceId, isCover);
            }

            Recipe.Images.Add(new RecipeImageDto
            {
                ImageUrl = url,
                DisplayOrder = Recipe.Images.Count + 1,
                IsCover = isCover
            });
        }

        protected void SetAsCover(RecipeImageDto selected)
        {
            foreach (var img in Recipe.Images)
                img.IsCover = false;

            selected.IsCover = true;
        }

        /// <summary>
        /// Call this method after creating the recipe to move any temp files to permanent storage.
        /// </summary>
        public async Task FinalizeImageUploadAsync(long newRecipeId)
        {
            await _uploadService.FinalizeUploadAsync(TempReferenceId, newRecipeId);

            foreach (var image in Recipe.Images)
            {
                if (image.ImageUrl.Contains("TempRecipeImages"))
                {
                    var updatedPath = image.ImageUrl
                        .Replace("TempRecipeImages", "RecipeImages")
                        .Replace(TempReferenceId.ToString(), newRecipeId.ToString());

                    image.ImageUrl = updatedPath;
                }
            }

            Recipe.Id = newRecipeId;
        }

        protected void Cancel()
        {
            JS.InvokeVoidAsync("history.back");
        }
    }
}
