using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.VoidGlass.VGCarousel
{
    public class VGCarouselBase : RSComponentBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;

        [Parameter] public List<RecipeImageDto> Images { get; set; } = new();
        [Parameter] public string? AriaLabel { get; set; } = "Image Carousel";

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

        protected async Task ScrollToSlide(int slideNumber)
        {
            await JS.InvokeVoidAsync("scrollToCarouselSlide", slideNumber);
        }
    }
}
