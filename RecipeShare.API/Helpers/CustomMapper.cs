using RecipeShare.API.Data.Models;
using RecipeShare.Models.Shared;
using RecipeShare.Utils.Helpers;

namespace RecipeShare.API.Helpers
{
    public static class CustomMapper
    {
        public static class RecipeMapper
        {
            public static RecipeDto ToDto(Recipe recipe, string? userId = null)
            {
                return new RecipeDto
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Ingredients = recipe.Ingredients,
                    Steps = string.Join("\n", recipe.RecipeSteps
                        .OrderBy(s => s.StepNumber)
                        .Select(s => s.Instruction)),
                    CookingTime = recipe.CookingTime,
                    DietaryTags = recipe.DietaryTags,
                    FavouriteCount = recipe.RecipeFavourites?.Count ?? 0,
                    Tags = recipe.Tags.Select(x => TagMapper.ToDto(x)).ToList(),
                    IsFavouritedByUser = !string.IsNullOrWhiteSpace(userId)
                        && recipe.RecipeFavourites!.Any(f => f.UserId == userId),
                    StructuredSteps = recipe.RecipeSteps
                        .OrderBy(s => s.StepNumber)
                        .Select(s => new RecipeStepDto
                        {
                            Id = s.Id,
                            RecipeId = s.RecipeId,
                            Order = s.StepNumber,
                            Instruction = s.Instruction
                        }).ToList(),
                    Images = recipe.RecipeImages
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new RecipeImageDto
                        {
                            Id = i.Id,
                            RecipeId = i.RecipeId,
                            ImageUrl = i.ImageUrl,
                            Caption = i.Caption,
                            DisplayOrder = i.DisplayOrder
                        }).ToList()
                };
            }

            public static RecipeTileDto ToTileDto(Recipe recipe, string? userId = null)
            {
                return new RecipeTileDto
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    CookingTime = recipe.CookingTime,
                    FavouriteCount = recipe.RecipeFavourites?.Count ?? 0,
                    Ingredients = recipe.Ingredients,
                    Tags = recipe.Tags.Select(x => TagMapper.ToDto(x)).ToList(),
                    IsFavouritedByUser = !string.IsNullOrWhiteSpace(userId)
                        && recipe.RecipeFavourites!.Any(f => f.UserId == userId),
                    CoverImageUrl = recipe.RecipeImages
                        .Where(x => x.IsCover)
                        .OrderBy(i => i.DisplayOrder)
                        .FirstOrDefault()?.ImageUrl ?? string.Empty
                };
            }

            public static Recipe ToEntity(RecipeDto dto, string? UserId)
            {
                return new Recipe
                {
                    Id = dto.Id,
                    UserId = UserId,
                    Title = dto.Title,
                    Ingredients = dto.Ingredients,
                    Steps = dto.Steps,
                    CookingTime = dto.CookingTime,
                    DietaryTags = dto.DietaryTags,

                    RecipeSteps = dto.StructuredSteps
                        .OrderBy(s => s.Order)
                        .Select(s => new RecipeStep
                        {
                            Id = s.Id,
                            RecipeId = dto.Id,
                            StepNumber = s.Order,
                            Instruction = s.Instruction
                        }).ToList(),

                    RecipeImages = dto.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new RecipeImage
                        {
                            Id = i.Id,
                            RecipeId = dto.Id,
                            ImageUrl = i.ImageUrl,
                            Caption = i.Caption,
                            DisplayOrder = i.DisplayOrder
                        }).ToList()
                };
            }

            public static void UpdateRecipeEntity(Recipe entity, RecipeDto dto)
            {
                entity.Title = dto.Title;
                entity.Ingredients = dto.Ingredients;
                entity.Steps = dto.Steps;
                entity.CookingTime = dto.CookingTime;
                entity.DietaryTags = dto.DietaryTags;

                // Optionally clear and reset RecipeSteps if full overwrite:
                if (dto.StructuredSteps?.Any() == true)
                {
                    entity.RecipeSteps = dto.StructuredSteps
                        .OrderBy(s => s.Order)
                        .Select(s => new RecipeStep
                        {
                            Id = s.Id,
                            RecipeId = dto.Id,
                            StepNumber = s.Order,
                            Instruction = s.Instruction
                        }).ToList();
                }

                // Same for images
                if (dto.Images?.Any() == true)
                {
                    entity.RecipeImages = dto.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new RecipeImage
                        {
                            Id = i.Id,
                            RecipeId = dto.Id,
                            ImageUrl = i.ImageUrl,
                            Caption = i.Caption,
                            DisplayOrder = i.DisplayOrder
                        }).ToList();
                }
            }

            public static void UpdateRecipeEntitySmart(Recipe entity, RecipeDto dto)
            {
                // Scalar field updates
                entity.Title = dto.Title;
                entity.Ingredients = dto.Ingredients;
                entity.Steps = dto.Steps;
                entity.CookingTime = dto.CookingTime;
                entity.DietaryTags = dto.DietaryTags;

                // --- Tags ---
                var incomingTagIds = dto.Tags.Select(t => t.Id).ToHashSet();
                var existingTags = entity.Tags.ToList();

                // Remove tags not in incoming list
                foreach (var tag in existingTags)
                {
                    if (!incomingTagIds.Contains(tag.Id))
                        entity.Tags.Remove(tag);
                }

                // Add tags not yet attached
                foreach (var incoming in incomingTagIds)
                {
                    if (!entity.Tags.Any(t => t.Id == incoming))
                    {
                        entity.Tags.Add(TagMapper.ToEntity(dto.Tags.First(x => x.Id == incoming)));
                    }
                }

                // --- Steps ---
                var incomingSteps = dto.StructuredSteps.OrderBy(s => s.Order).ToList();
                var existingSteps = entity.RecipeSteps.ToList();

                // Update or Add
                foreach (var incoming in incomingSteps)
                {
                    var existing = existingSteps.FirstOrDefault(s => s.Id == incoming.Id);
                    if (existing != null)
                    {
                        existing.StepNumber = incoming.Order;
                        existing.Instruction = incoming.Instruction;
                    }
                    else
                    {
                        entity.RecipeSteps.Add(new RecipeStep
                        {
                            StepNumber = incoming.Order,
                            Instruction = incoming.Instruction,
                            RecipeId = entity.Id
                        });
                    }
                }

                // Remove orphaned
                var incomingStepIds = incomingSteps.Select(s => s.Id).ToHashSet();
                var stepsToRemove = existingSteps.Where(s => s.Id != 0 && !incomingStepIds.Contains(s.Id)).ToList();

                foreach (var step in stepsToRemove)
                {
                    entity.RecipeSteps.Remove(step);
                }

                // --- Images ---
                var incomingImages = dto.Images.OrderBy(i => i.DisplayOrder).ToList();
                var existingImages = entity.RecipeImages.ToList();

                foreach (var incoming in incomingImages)
                {
                    var existing = existingImages.FirstOrDefault(i => i.Id == incoming.Id);
                    if (existing != null)
                    {
                        existing.ImageUrl = incoming.ImageUrl;
                        existing.Caption = incoming.Caption;
                        existing.DisplayOrder = incoming.DisplayOrder;
                    }
                    else
                    {
                        entity.RecipeImages.Add(new RecipeImage
                        {
                            ImageUrl = incoming.ImageUrl,
                            Caption = incoming.Caption,
                            DisplayOrder = incoming.DisplayOrder,
                            RecipeId = entity.Id
                        });
                    }
                }

                var incomingImageIds = incomingImages.Select(i => i.Id).ToHashSet();

                // ✅ Ensure only ONE image is marked as cover
                var coverSet = entity.RecipeImages.Count(i => i.IsCover);

                // 1. If multiple are marked as cover, keep the first one only
                if (coverSet > 1)
                {
                    var first = entity.RecipeImages.FirstOrDefault(i => i.IsCover);
                    foreach (var img in entity.RecipeImages)
                    {
                        img.IsCover = (img == first);
                    }
                }

                // 2. If NONE marked as cover, fallback to first Additional image
                if (coverSet == 0 && entity.RecipeImages.Count > 0)
                {
                    var first = entity.RecipeImages.OrderBy(i => i.DisplayOrder).FirstOrDefault();
                    if (first != null)
                        first.IsCover = true;
                }

                var imagesToRemove = existingImages.Where(i => i.Id != 0 && !incomingImageIds.Contains(i.Id)).ToList();

                foreach (var img in imagesToRemove)
                {
                    entity.RecipeImages.Remove(img);
                }
            }
        }

        public static class TagMapper
        {
            public static TagDto ToDto(Tag tag)
            {
                return new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = string.IsNullOrWhiteSpace(tag.ColorHex)
                        ? TagColorGenerator.GenerateColor(tag.Name)
                        : tag.ColorHex,
                    Description = tag.Description
                };
            }

            public static Tag ToEntity(TagDto dto)
            {
                return new Tag
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    ColorHex = dto.Color,
                    Description = dto.Description
                };
            }

            public static void UpdateTagEntity(Tag entity, TagDto dto)
            {
                entity.Name = dto.Name;
                entity.ColorHex = dto.Color;
                entity.Description = dto.Description;
            }
        }
    }
}