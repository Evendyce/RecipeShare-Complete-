using Microsoft.AspNetCore.Components;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;
using RecipeShare.Web.Services;
using static System.Net.WebRequestMethods;

namespace RecipeShare.Web.Components.Pages.Recipes
{
    public class RecipeFilterBarBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        //I N J E C T I O N S
        [Inject]
        protected ITagService _tagService { get; set; }

        protected RecipeSearchDto filter = new();
        protected List<TagDto> tags = new();

        [Parameter]
        public EventCallback<RecipeSearchDto> OnSearch { get; set; }
        [Parameter]
        public string Title { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //INDICATE DATA LOAD STARTED
                IsLoading = true;
                await InvokeAsync(StateHasChanged);

                //LOAD INITIAL PAGE/COMPONENT DATA HERE
                tags = await _tagService.GetAllTagsAsync();

                //INDICATE DATA LOAD FINISHED
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task SubmitFilter()
        {
            await OnSearch.InvokeAsync(filter);
        }

        protected void ToggleTag(string tagName)
        {
            if (filter.Tags.Contains(tagName))
                filter.Tags.Remove(tagName);
            else
                filter.Tags.Add(tagName);
        }
    }
}
