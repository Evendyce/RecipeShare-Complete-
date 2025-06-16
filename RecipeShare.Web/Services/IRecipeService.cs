using Microsoft.EntityFrameworkCore;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Data;

namespace RecipeShare.Web.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeTileDto>> GetRecipeTilesAsync(RecipeSearchDto filter);
        Task<List<RecipeTileDto>> GetRecipeTilesForUserAsync(RecipeSearchDto filter);
        Task<List<RecipeTileDto>> GetMyRecipeTilesAsync(RecipeSearchDto filter);
        Task<RecipeDto?> GetRecipeByIdAsync(long id, string? username);
        Task<long> CreateRecipeAsync(RecipeDto model);
        Task<bool> UpdateRecipeAsync(RecipeDto model);
        Task<bool> DeleteRecipeAsync(long recipeId);
        Task<bool> ToggleFavouriteAsync(FavouriteToggleRequestDto dto);
    }

    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _http;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(HttpClient http, ILogger<RecipeService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<RecipeTileDto>> GetRecipeTilesAsync(RecipeSearchDto filter)
        {
            try
            {
                var url = $"/api/recipes?{BuildQueryString(filter)}";

                var response = await _http.GetFromJsonAsync<List<RecipeTileDto>>(url);

                return response ?? new List<RecipeTileDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to load recipe tiles");
                return new List<RecipeTileDto>();
            }
        }

        public async Task<List<RecipeTileDto>> GetRecipeTilesForUserAsync(RecipeSearchDto filter)
        {
            try
            {
                var url = $"/api/recipes/recipes-for-user?{BuildQueryString(filter)}";

                var response = await _http.GetFromJsonAsync<List<RecipeTileDto>>(url);

                return response ?? new List<RecipeTileDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to load recipe tiles");
                return new List<RecipeTileDto>();
            }
        }

        public async Task<List<RecipeTileDto>> GetMyRecipeTilesAsync(RecipeSearchDto filter)
        {
            try
            {
                var url = $"/api/recipes/my-recipes?{BuildQueryString(filter)}";

                var response = await _http.GetFromJsonAsync<List<RecipeTileDto>>(url);

                return response ?? new List<RecipeTileDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to load recipe tiles");
                return new List<RecipeTileDto>();
            }
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(long id, string? username)
        {
            try
            {
                var url = $"/api/recipes/{id}";

                // Append username if available
                if (!string.IsNullOrWhiteSpace(username))
                {
                    url += $"?Username={Uri.EscapeDataString(username)}";
                }

                var response = await _http.GetFromJsonAsync<RecipeDto>(url);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to fetch recipe with ID {RecipeId}", id);
                return null;
            }
        }

        public async Task<long> CreateRecipeAsync(RecipeDto model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/recipes", model);

                if (response.IsSuccessStatusCode)
                {
                    var id = await response.Content.ReadFromJsonAsync<long>();
                    return id;
                }

                _logger.LogWarning("[RecipeService] CreateRecipeAsync responded with {StatusCode}", response.StatusCode);
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to create recipe");
                return 0;
            }
        }

        public async Task<bool> UpdateRecipeAsync(RecipeDto model)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"/api/recipes/{model.Id}", model);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                _logger.LogWarning("[RecipeService] UpdateRecipeAsync responded with {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to update recipe {RecipeId}", model.Id);
                return false;
            }
        }

        public async Task<bool> DeleteRecipeAsync(long recipeId)
        {
            try
            {
                var response = await _http.DeleteAsync($"/api/recipes/{recipeId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                _logger.LogWarning("[RecipeService] DeleteRecipeAsync responded with {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to delete recipe with ID {RecipeId}", recipeId);
                return false;
            }
        }

        public async Task<bool> ToggleFavouriteAsync(FavouriteToggleRequestDto dto)
        {
            try
            {
                var url = $"/api/recipes/favourite-recipe";
                var content = JsonContent.Create(dto);

                var response = await _http.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                _logger.LogWarning("[RecipeService] ToggleFavouriteAsync responded with {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to favourite recipe");
                return false;
            }
        }

        private static string BuildQueryString(RecipeSearchDto filter)
        {
            var queryParams = new List<string> { "useTile=true" };

            if (!string.IsNullOrWhiteSpace(filter.Title))
                queryParams.Add($"Title={Uri.EscapeDataString(filter.Title)}");

            if (!string.IsNullOrWhiteSpace(filter.Username))
                queryParams.Add($"Username={Uri.EscapeDataString(filter.Username)}");

            if (filter.Tags?.Any() == true)
            {
                foreach (var tag in filter.Tags)
                    queryParams.Add($"Tags={Uri.EscapeDataString(tag)}");
            }

            if (filter.MinCookingTime.HasValue)
                queryParams.Add($"MinCookingTime={filter.MinCookingTime.Value}");

            if (filter.MaxCookingTime.HasValue)
                queryParams.Add($"MaxCookingTime={filter.MaxCookingTime.Value}");

            return string.Join("&", queryParams);
        }
    }
}
