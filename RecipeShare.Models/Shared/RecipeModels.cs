using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeShare.Models.Shared
{
    public class RecipeDto
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty; // Flat string for spec compliance
        public int CookingTime { get; set; }
        public string DietaryTags { get; set; } = string.Empty;

        public List<RecipeStepDto> StructuredSteps { get; set; } = new();
        public List<RecipeImageDto> Images { get; set; } = new();

        public int FavouriteCount { get; set; } // Computed only, not editable
    }

    public class RecipeStepDto
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }

        public int Order { get; set; }
        public string Instruction { get; set; } = string.Empty;
    }

    public class RecipeImageDto
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class RecipeTileDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public List<TagDto> Tags { get; set; } = new();
        public int CookingTime { get; set; }
        public int FavouriteCount { get; set; }
    }

    public class RecipeSearchDto
    {
        public string? Title { get; set; }
        public string? Tag { get; set; }          // Single tag for basic use
        public List<string>? Tags { get; set; } = new();   // Optional multi-tag support
        public int? MinCookingTime { get; set; }
        public int? MaxCookingTime { get; set; }
        public string? Ingredient { get; set; }   // Optional: filter by included ingredient keyword
    }
}
