using RecipeShare.Web.Helpers.System;

namespace RecipeShare.Web.Components.Layout
{
    public class MainLayoutBase : RSLayoutBase
    {
        //L O A D I N G   I N D I C A T O R
        protected bool IsLoading { get; set; } = false;
    }
}
