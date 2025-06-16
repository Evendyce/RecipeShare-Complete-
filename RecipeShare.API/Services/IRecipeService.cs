using Microsoft.EntityFrameworkCore;
using RecipeShare.API.Data;
using RecipeShare.Models.Shared;
using System;
using RecipeShare.API.Helpers;
using RecipeShare.API.Data.Models;

namespace RecipeShare.API.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeDto>> GetAllRecipesAsync(RecipeSearchDto? filter);
        Task<List<RecipeTileDto>> GetAllTilesAsync(RecipeSearchDto? filter);
        Task<List<RecipeTileDto>> GetAllTilesForUserAsync(RecipeSearchDto? filter);
        Task<List<RecipeTileDto>> GetMyRecipesTilesAsync(RecipeSearchDto? filter);
        Task<RecipeDto?> GetByIdAsync(long id, string? username);
        Task<RecipeDto> CreateAsync(RecipeDto dto);
        Task<bool> UpdateAsync(long id, RecipeDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> ToggleFavouriteAsync(FavouriteToggleRequestDto dto);
    }

    public class RecipeService : IRecipeService
    {
        private readonly IDbContextFactory<RecipeShareContext> _contextFactory;

        public RecipeService(IDbContextFactory<RecipeShareContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<RecipeDto>> GetAllRecipesAsync(RecipeSearchDto? filter)
        {
            await using var ctx = await _contextFactory.CreateDbContextAsync();

            var user = await ctx.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == filter.Username);

            var query = ctx.Recipes
                .Include(r => r.Tags)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeImages)
                .Include(r => r.RecipeFavourites)
                .AsQueryable();

            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Title))
                    query = query.Where(r => r.Title.Contains(filter.Title));

                if (!string.IsNullOrWhiteSpace(filter.Tag))
                    query = query.Where(r => r.Tags.Any(t => t.Name == filter.Tag));

                if (filter.Tags?.Any() == true)
                    query = query.Where(r => r.Tags.Any(t => filter.Tags.Contains(t.Name)));

                if (!string.IsNullOrWhiteSpace(filter.Ingredient))
                    query = query.Where(r => r.Ingredients.Contains(filter.Ingredient));

                if (filter.MinCookingTime.HasValue)
                    query = query.Where(r => r.CookingTime >= filter.MinCookingTime.Value);

                if (filter.MaxCookingTime.HasValue)
                    query = query.Where(r => r.CookingTime <= filter.MaxCookingTime.Value);
            }

            var returnData = await query
                .OrderBy(r => r.Title)
                .Select(r => CustomMapper.RecipeMapper.ToDto(r, (user != null ? user.Id : "")))
                .ToListAsync();

            return returnData;
        }

        public async Task<List<RecipeTileDto>> GetAllTilesAsync(RecipeSearchDto? filter)
        {
            await using var ctx = await _contextFactory.CreateDbContextAsync();

            var user = await ctx.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == filter.Username);

            var query = ctx.Recipes
                .Include(r => r.Tags)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeImages)
                .Include(r => r.RecipeFavourites)
                .AsQueryable();

            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Title))
                    query = query.Where(r => r.Title.Contains(filter.Title));

                if (!string.IsNullOrWhiteSpace(filter.Tag))
                    query = query.Where(r => r.Tags.Any(t => t.Name == filter.Tag));

                if (filter.Tags?.Any() == true)
                    query = query.Where(r => r.Tags.Any(t => filter.Tags.Contains(t.Name)));

                if (!string.IsNullOrWhiteSpace(filter.Ingredient))
                    query = query.Where(r => r.Ingredients.Contains(filter.Ingredient));

                if (filter.MinCookingTime.HasValue)
                    query = query.Where(r => r.CookingTime >= filter.MinCookingTime.Value);

                if (filter.MaxCookingTime.HasValue)
                    query = query.Where(r => r.CookingTime <= filter.MaxCookingTime.Value);
            }

            var returnData = await query
                .OrderBy(r => r.Title)
                .Select(r => CustomMapper.RecipeMapper.ToTileDto(r, (user != null ? user.Id : "")))
                .ToListAsync();

            return returnData;
        }

        public async Task<List<RecipeTileDto>> GetAllTilesForUserAsync(RecipeSearchDto? filter)
        {
            await using var ctx = await _contextFactory.CreateDbContextAsync();

            var user = await ctx.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == filter.Username);

            if (user != null)
            {
                var query = ctx.Recipes
                    .Include(r => r.Tags)
                    .Include(r => r.RecipeSteps)
                    .Include(r => r.RecipeImages)
                    .Include(r => r.RecipeFavourites)
                    .Where(x => x.RecipeFavourites!.Any(f => f.UserId == user.Id))
                    .AsQueryable();

                var returnData = await query
                    .OrderBy(r => r.Title)
                    .Select(r => CustomMapper.RecipeMapper.ToTileDto(r, (user != null ? user.Id : "")))
                    .ToListAsync();

                return returnData;
            }

            return new List<RecipeTileDto>();
        }

        public async Task<List<RecipeTileDto>> GetMyRecipesTilesAsync(RecipeSearchDto? filter)
        {
            await using var ctx = await _contextFactory.CreateDbContextAsync();

            var user = await ctx.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == filter.Username);

            if (user != null)
            {
                var query = ctx.Recipes
                    .Include(r => r.Tags)
                    .Include(r => r.RecipeSteps)
                    .Include(r => r.RecipeImages)
                    .Include(r => r.RecipeFavourites)
                    .Where(x => x.UserId == user.Id)
                    .AsQueryable();

                var returnData = await query
                    .OrderBy(r => r.Title)
                    .Select(r => CustomMapper.RecipeMapper.ToTileDto(r, (user != null ? user.Id : "")))
                    .ToListAsync();

                return returnData;
            }

            return new List<RecipeTileDto>();
        }

        public async Task<RecipeDto?> GetByIdAsync(long id, string? username)
        {
            await using var ctx = await _contextFactory.CreateDbContextAsync();

            var user = await ctx.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == username);

            var recipe = await ctx.Recipes
                .Include(r => r.Tags)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeImages)
                .Include(r => r.RecipeFavourites)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return null;

            return CustomMapper.RecipeMapper.ToDto(recipe, (user != null ? user.Id : ""));
        }

        public async Task<RecipeDto> CreateAsync(RecipeDto dto)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var entity = CustomMapper.RecipeMapper.ToEntity(dto);

            context.Recipes.Add(entity);
            await context.SaveChangesAsync();

            dto.Id = entity.Id;

            return dto;
        }

        public async Task<bool> UpdateAsync(long id, RecipeDto dto)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var r = await context.Recipes
                .Include(x => x.RecipeSteps)
                .Include(x => x.RecipeImages)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (r == null)
                return false;

            CustomMapper.RecipeMapper.UpdateRecipeEntitySmart(r, dto);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var r = await context.Recipes.FindAsync(id);
            if (r == null) return false;

            context.Recipes.Remove(r);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleFavouriteAsync(FavouriteToggleRequestDto dto)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var user = await context.AspNetUsers
                .FirstOrDefaultAsync(x => x.UserName == dto.Username);

            if (user == null) return false;

            var recipe = await context.Recipes
                .Include(x => x.RecipeFavourites)
                .FirstOrDefaultAsync(x => x.Id == dto.RecipeId);

            if (recipe == null) return false;

            // Check if user already favourited
            var existing = recipe.RecipeFavourites
                .FirstOrDefault(f => f.UserId == user.Id);

            if (existing != null)
            {
                // Unfavourite
                context.RecipeFavourites.Remove(existing);
            }
            else
            {
                // Add favourite
                var fav = new RecipeFavourite
                {
                    RecipeId = recipe.Id,
                    UserId = user.Id
                };

                await context.RecipeFavourites.AddAsync(fav);
            }

            await context.SaveChangesAsync();
            return true;
        }

    }
}
